namespace Project.src.Infra.Repository.Interfaces;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public bool VerifyPassword(string encode, string password);
}