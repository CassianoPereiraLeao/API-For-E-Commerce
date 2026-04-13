namespace Project.src.DTOs;

public class UpdateUserDTO
{
    public UpdateUserDTO(string name, string email, string password, string role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public string? Name { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string? Password { get; set; } = default!;
    public string? Role { get; set; } = default!;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
