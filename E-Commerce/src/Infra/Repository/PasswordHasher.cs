using Microsoft.Extensions.Options;
using Isopoh.Cryptography.Argon2;
using System.Text;
using Project.src.Infra.Repository.Interfaces;

namespace Project.src.Infra.Repository;

public class PasswordHasher : IPasswordHasher
{
    private readonly Argon2Settings _settings;

    public PasswordHasher(IOptions<Argon2Settings> settings)
    {
        _settings = settings.Value;
    }

    public string HashPassword(string password) { return Argon2.Hash(password, _settings.TimeCost, _settings.MemoryCost, _settings.Lanes); }

    public bool VerifyPassword(string encode, string password) { return Argon2.Verify(encode, password); }
}