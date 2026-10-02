using Microsoft.AspNetCore.Http;

namespace FastRouter;

/// <summary>
/// Implement this interface in a class inside your Routes folder/namespace.
/// The class is created per request via dependency injection, so constructor
/// injection works as usual.
/// </summary>
public interface IRouteHandler
{
    Task<IResult> HandleAsync(HttpContext context);
}
