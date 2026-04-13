using Project.src.Routes.Request;

namespace Project.src.Routes.Response;

public class ApiResponse<T>
{
    public string Status { get; set; } = default!;
    public string Message { get; set; } = default!;
    public T? Data { get; set; } = default!;
    public ApiMetadata? Meta { get; set; }
    public List<ApiErrorHandler>? Errors { get; set; }
}
