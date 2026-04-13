using System.ComponentModel.DataAnnotations;

namespace Project.src.Routes.Request.Login;

public class UserRequestLogin
{
    
    [Required]
    [EmailAddress]
    [MinLength(10)]
    [MaxLength(255)]
    public string Email { get; set; } = default!;
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = default!;
}
