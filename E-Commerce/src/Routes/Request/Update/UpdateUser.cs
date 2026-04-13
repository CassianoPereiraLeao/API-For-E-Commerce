using System.ComponentModel.DataAnnotations;

namespace Project.src.Routes.Request.Update;

public class UserRequestUpdate
{
    [MinLength(3)]
    [MaxLength(255)]
    public string? Name { get; set; }
    [EmailAddress]
    [MinLength(10)]
    [MaxLength(255)]
    public string? Email { get; set; }
    [MinLength(8)]
    public string? Password { get; set; }
    public string? Role { get; set; }
}
