namespace Project.src.DTOs;

public class UserLoginDTO
{
    public string Token { get; set; } = string.Empty;
    public UserDTO User { get; set; } = null!;
}
