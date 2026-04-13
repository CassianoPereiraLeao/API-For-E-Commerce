using System.ComponentModel.DataAnnotations;

namespace Project.src.Routes.Request.Login;

public class RefreshTokenRequest
{
    [Required]
    public string AccessToken { get; set; } = default!;
    [Required]
    public string RefreshToken { get; set; } = default!;
}