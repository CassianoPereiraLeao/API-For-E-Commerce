using Microsoft.AspNetCore.Mvc;

namespace Project.src.Routes;

public static class ProductRoute
{
    public static void ProductRoutes(this WebApplication app)
    {
        RouteGroupBuilder route = app.MapGroup("/api/product");

        route.MapGet("/", async (
            [FromQuery] int? page,
            [FromQuery] string? name,
            [FromQuery] int? minPrice,
            [FromQuery] int? maxPrice,
            [FromQuery] int pageSize
            ) =>
        {
            return Results.Ok();
        }).WithTags("Product");
    }
}