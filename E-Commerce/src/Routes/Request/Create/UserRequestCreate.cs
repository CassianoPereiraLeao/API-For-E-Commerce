using System.ComponentModel.DataAnnotations;

namespace Project.src.Routes.Request.Create;

public class UserRequestCreate
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; set; } = default!;
    [Required]
    [EmailAddress]
    [MinLength(10)]
    [MaxLength(255)]
    public string Email { get; set; } = default!;
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = default!;
    [Required]
    public string Role { get; set; } = default!;
}