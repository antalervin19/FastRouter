using System.Reflection;

namespace FastRouter;

public sealed class FastRouterOptions
{
    /// <summary>
    /// Root namespace that contains your handler classes. Default: "Routes".
    /// "/users/me" resolves to the class "Routes.Users.Me".
    /// The match is case-insensitive and also works when the namespace is
    /// prefixed by your project namespace (e.g. "MyApp.Routes.Users.Me").
    /// </summary>
    public string Namespace { get; set; } = "Routes";

    /// <summary>
    /// Assembly to scan for handlers. Default: the entry assembly.
    /// </summary>
    public Assembly? Assembly { get; set; }

    /// <summary>
    /// Optional prefix added to every route, e.g. "/api".
    /// </summary>
    public string Prefix { get; set; } = "";
}
