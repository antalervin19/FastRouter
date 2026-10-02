using Microsoft.AspNetCore.Builder;

namespace FastRouter;

public static class FastRouterExtensions
{
    /// <summary>
    /// Shortcut: <c>app.UseFastRouter(r =&gt; { r.Get("/hello"); })</c>
    /// </summary>
    public static WebApplication UseFastRouter(
        this WebApplication app,
        Action<FastRouter> declare,
        Action<FastRouterOptions>? configure = null)
    {
        var router = new FastRouter(app, configure);
        declare(router);
        router.Load();
        return app;
    }
}
