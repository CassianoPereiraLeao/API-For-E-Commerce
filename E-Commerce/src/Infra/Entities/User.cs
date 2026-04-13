using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Project.src.Infra.Repository;

namespace Project.src.Infra.Entities;

public class User
{
    public User(string name, string email, string password, string role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    [Key]
    public Guid Id { get; init; }= Guid.NewGuid();
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Name { get; private set; } = default!;
    [Required]
    [EmailAddress]
    [MinLength(10)]
    [MaxLength(255)]
    public string Email { get; private set; } = default!;
    [Required]
    [MinLength(8)]
    public string Password { get; private set; } = default!;
    [Required]
    public string Role { get; private set; } = default!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void UpdateName(string name) => Name = name;
    public void UpdateEmail(string email) => Email = email;
    public void UpdatePassword(string password) => Password = password;
    public void UpdateRole(string role) => Role = role;
}
