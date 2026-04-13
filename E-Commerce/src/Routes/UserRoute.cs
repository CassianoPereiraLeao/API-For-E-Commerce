using Project.src.Routes.Request.Update;
using Microsoft.AspNetCore.Mvc;
using Project.src.Routes.Request.Create;
using Project.src.Routes.Request.Login;
using Project.src.Routes.Response;
using Project.src.Domain.Interfaces;
using Project.src.DTOs;
using Project.src.Middlewares;
using Project.src.Infra.Entities;

namespace Project.src.Routes;

public static class UserRoute
{
    public static void UserRoutes(this WebApplication app)
    {
        RouteGroupBuilder route = app.MapGroup("/api/user");

        route.MapGet("/", async ([FromQuery] int? page, [FromQuery] int? limit, IUserService userService) =>
        {
            if(page < 1 && page != null)
                page = 1;
            if(limit < 1 && limit != null)
                limit = 1;

            var response = await userService.GetAllUser(page, limit);
            if(response.Status == "error") 
                return Results.BadRequest(response);

            return Results.Ok(response);
        }).WithTags("User").RequireAuthorization();

        route.MapGet("/{id:guid}", async (Guid id, IUserService userService) =>
        {
            ApiResponse<UserDTO> response = await userService.GetUserById(id);

            if(response.Status == "error")
                return Results.NotFound(response);

            return Results.Ok(response);
        }).WithTags("User").RequireAuthorization("AdminOnly");

        route.MapPost("/create", async ([FromBody] UserRequestCreate userRequestCreate, 
            IUserService userService) =>
        {
            var response = await userService.CreateUser(userRequestCreate);

            if(response.Status == "error" || response.Status == "invalid")
                return Results.BadRequest(response);

            return Results.Created();
        }).WithTags("User");

        route.MapPost("/login", async ([FromBody] UserRequestLogin userRequestLogin, IUserService userService, 
        HttpContext context, JwtService tokenizer) =>
        {
            var response = await userService.LoginUser(userRequestLogin);

            if(response.Status == "error")
                return Results.Json(response, statusCode: 401);

            if(response.Status == "invalid")
                return Results.BadRequest(response);

            string refreshToken = tokenizer.GenerateRefreshToken();

            await userService.RefreshTokenUser(refreshToken);

            var cookiesOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            context.Response.Cookies.Append("refreshToken", refreshToken, cookiesOptions);

            return Results.Ok(response);
        }).WithTags("User");

        route.MapPatch("/{id:guid}", async ([FromBody] UserRequestUpdate userRequestUpdate, Guid id, IUserService userService) =>
        {

        }).WithTags("User").RequireAuthorization("AdminOnly");

        route.MapDelete("/{id:guid}", async (Guid id, IUserService userService) =>
        {
            var response = await userService.DeleteUser(id);

            if(response.Status == "error")
                return Results.NotFound();

            return Results.Ok(response);
        }).WithTags("User").RequireAuthorization("AdminOnly");

        route.MapPost("/refresh", async (HttpContext context, JwtService tokenizer, IUserService userService) =>
        {
            var token = context.Request.Cookies["refreshToken"];
            if(token == null)
                return Results.BadRequest();

            await userService.RefreshTokenUser(token);
            string refreshToken = tokenizer.GenerateRefreshToken();

            return Results.Ok();
        });
    }
}
