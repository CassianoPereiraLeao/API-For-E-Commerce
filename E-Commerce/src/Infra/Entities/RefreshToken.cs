using Microsoft.AspNetCore.Identity;

namespace Project.src.Infra.Entities;

public class RefreshToken
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Token { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime Expires { get; set; } = default!;
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public Guid UserId { get; set; } = default!;
}
