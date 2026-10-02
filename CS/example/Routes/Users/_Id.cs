using FastRouter;

namespace Example.Routes.Users;

// Handles /users/{id}  (file/class name: _Id)
public class _Id : IRouteHandler
{
    public Task<IResult> HandleAsync(HttpContext context)
    {
        var id = context.Request.RouteValues["id"]?.ToString();
        return Task.FromResult(Results.Ok(new { id }));
    }
}
