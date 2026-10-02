using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FastRouter;

/// <summary>
/// Declare routes in one place, let FastRouter wire up the handler classes.
/// </summary>
public sealed class FastRouter
{
    private readonly IEndpointRouteBuilder _endpoints;
    private readonly FastRouterOptions _options;
    private readonly List<(string Method, string Path)> _routes = new();
    private Dictionary<string, Type>? _handlers;

    public FastRouter(IEndpointRouteBuilder endpoints, Action<FastRouterOptions>? configure = null)
    {
        _endpoints = endpoints ?? throw new ArgumentNullException(nameof(endpoints));
        _options = new FastRouterOptions();
        configure?.Invoke(_options);
    }

    public FastRouter Get(string path) => Add(HttpMethods.Get, path);
    public FastRouter Post(string path) => Add(HttpMethods.Post, path);
    public FastRouter Put(string path) => Add(HttpMethods.Put, path);
    public FastRouter Patch(string path) => Add(HttpMethods.Patch, path);
    public FastRouter Delete(string path) => Add(HttpMethods.Delete, path);
    public FastRouter Head(string path) => Add(HttpMethods.Head, path);
    public FastRouter Options(string path) => Add(HttpMethods.Options, path);

    private FastRouter Add(string method, string path)
    {
        if (string.IsNullOrWhiteSpace(path) || path[0] != '/')
            throw new ArgumentException($"Route path must start with '/': \"{path}\"", nameof(path));

        _routes.Add((method, path));
        return this;
    }

    /// <summary>
    /// Resolves a handler class for every declared route and maps it.
    /// Throws if a handler cannot be found, so mistakes surface at startup.
    /// </summary>
    public FastRouter Load()
    {
        _handlers ??= ScanHandlers();

        foreach (var (method, path) in _routes)
        {
            var key = NormalizeKey(path);
            if (!_handlers.TryGetValue(key, out var type))
                throw new InvalidOperationException(
                    $"FastRouter: no handler found for {method} {path}. " +
                    $"Expected an IRouteHandler class at \"{ExpectedName(path)}\".");

            var handlerType = type;
            _endpoints.MapMethods(_options.Prefix + path, new[] { method }, async (HttpContext ctx) =>
            {
                var handler = (IRouteHandler)ActivatorUtilities.CreateInstance(ctx.RequestServices, handlerType);
                var result = await handler.HandleAsync(ctx);
                await result.ExecuteAsync(ctx);
            });
        }

        _routes.Clear();
        return this;
    }

    // ---- handler discovery -------------------------------------------------

    private Dictionary<string, Type> ScanHandlers()
    {
        var assembly = _options.Assembly
            ?? Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException("FastRouter: could not determine the assembly to scan.");

        var root = "." + _options.Namespace.Trim('.') + ".";
        var map = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        foreach (var type in assembly.GetTypes())
        {
            if (!type.IsClass || type.IsAbstract || !typeof(IRouteHandler).IsAssignableFrom(type))
                continue;

            var full = "." + type.FullName;
            var idx = full.IndexOf(root, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) continue;

            // "MyApp.Routes.Users.Me" -> "users/me"
            var relative = full[(idx + root.Length)..];
            var parts = relative.Split('.').ToList();
            if (parts.Count > 1 && parts[^1].Equals("Index", StringComparison.OrdinalIgnoreCase))
                parts.RemoveAt(parts.Count - 1);   // Routes.Users.Index -> /users
            map[NormalizeKey(string.Join('/', parts))] = type;
        }

        return map;
    }

    // "/users/{id}" -> "users/_id", "/users/me" -> "users/me"
    private static string NormalizeKey(string path)
    {
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.StartsWith('{') && s.EndsWith('}') ? "_" + s[1..^1] : s);
        return string.Join('/', segments).ToLowerInvariant();
    }

    private string ExpectedName(string path)
    {
        var parts = NormalizeKey(path).Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => char.ToUpperInvariant(p[0]) + p[1..]);
        return _options.Namespace + "." + string.Join('.', parts);
    }
}
