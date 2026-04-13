namespace Project.src.Routes.Request;

public class ApiMetadata
{
    public int Page { get; set; } = default!;
    public int Per_page { get; set; } = default!;
    public int Total { get; set; } = default!;
    public int Total_pages { get; set; } = default!;
}
