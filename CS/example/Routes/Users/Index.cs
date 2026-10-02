using FastRouter;

namespace Example.Routes.Users;

// Handles POST /users
public class Index : IRouteHandler
{
    public Task<IResult> HandleAsync(HttpContext context)
        => Task.FromResult(Results.Created("/users/2", new { id = 2 }));
}
