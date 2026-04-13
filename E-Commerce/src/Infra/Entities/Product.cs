using System.ComponentModel.DataAnnotations;

namespace Project.src.Infra.Entities;

public class Products
{
    public Products(string name, int prize, int stock, bool active)
    {
        Name = name;
        Prize = prize;
        Stock = stock;
        Active = active;
    }

    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    [Required]
    [MaxLength(255)]
    public string Name { get; private set; } = default!;
    [Required]
    public int Prize { get; private set; } = default!;
    [Required]
    public int Stock { get; private set; } = default!;
    [Required]
    public bool Active { get; private set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}