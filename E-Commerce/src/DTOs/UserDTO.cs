namespace Project.src.DTOs;

public class UserDTO
{
    public UserDTO(Guid id, string name, string email, string role)
    {
        Id = id;
        Name = name;
        Email = email;
        Role = role;
    }
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}
