# FastRouter for ASP.NET Core

A small convention-based API router for `WebApplication`. Declare your routes in one place; FastRouter finds the handler class for each one.

## Install

```
dotnet add package antalervin19.FastRouter
```

## Quick start

```csharp
using FastRouter;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var router = new FastRouter.FastRouter(app);

router.Get("/hello");
router.Get("/users/me");
router.Get("/users/{id}");
router.Post("/users");

router.Load();
app.Run();
```

Or the one-liner:

```csharp
app.UseFastRouter(r =>
{
    r.Get("/hello");
    r.Get("/users/me");
});
```

## Handlers

Put one class per route in a `Routes` folder (namespace `*.Routes`):

```
Routes/
├── Hello.cs        -> /hello
└── Users/
    ├── Index.cs    -> /users
    ├── Me.cs       -> /users/me
    └── _Id.cs      -> /users/{id}
```

```csharp
namespace MyApp.Routes;

public class Hello : IRouteHandler
{
    public Task<IResult> HandleAsync(HttpContext context)
        => Task.FromResult(Results.Ok(new { message = "Hello from FastRouter!" }));
}
```

- Handlers are created per request through DI, so **constructor injection works**.
- `{param}` segments map to a class named `_param`.
- `Index` maps to its folder's path (`Users/Index.cs` → `/users`).
- Matching is case-insensitive.
- `Load()` throws at startup if a declared route has no handler.
- Supports GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS.

## Options

```csharp
new FastRouter.FastRouter(app, o =>
{
    o.Namespace = "Routes";   // root namespace to scan
    o.Prefix    = "/api";     // prefix for every route
    o.Assembly  = typeof(Program).Assembly;
});
```

## License

MIT
