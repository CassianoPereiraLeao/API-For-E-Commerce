using Project.src.Routes.Response;

namespace Project.src.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO CRÍTICO]: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>
            {
                Status = "error",
                Message = "An unexpected error occurred.",
                Errors = new List<ApiErrorHandler>
                {
                    new ApiErrorHandler
                    {
                        Field = "server",
                        Message = ex.Message
                    }
                }
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}