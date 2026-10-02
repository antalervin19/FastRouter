using FastRouter;

namespace Example.Routes.Users;

public class Me : IRouteHandler
{
    public Task<IResult> HandleAsync(HttpContext context)
        => Task.FromResult(Results.Ok(new { id = 1, name = "Me" }));
}
