using FastRouter;

namespace Example.Routes;

public class Hello : IRouteHandler
{
    public Task<IResult> HandleAsync(HttpContext context)
        => Task.FromResult(Results.Ok(new { message = "Hello from FastRouter!" }));
}
