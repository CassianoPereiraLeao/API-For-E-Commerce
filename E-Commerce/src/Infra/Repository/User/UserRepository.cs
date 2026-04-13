using Microsoft.EntityFrameworkCore;
using Project.src.DTOs;
using Project.src.Infra.Data;
using Project.src.Infra.Entities;
using Project.src.Infra.Repository.Interfaces;
using Project.src.Routes.Request.Update;

namespace Project.src.Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _hasher;
    public UserRepository(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _hasher = passwordHasher;
    }

    public async Task<bool> CreateUser(User user)
    {
        var existEmail = await _context.Users.AnyAsync(u => u.Email == user.Email);
        if(existEmail)
            return false;

        user.UpdatePassword(_hasher.HashPassword(user.Password));
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if(user == null)
            return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDTO>> GetAllUsers(int page, int limit)
    {
        var total = await _context.Users.CountAsync();

        var users = await _context.Users
                        .Skip((page - 1) * limit)
                        .Take(limit)
                        .Select(u => new UserDTO(u.Id, u.Name, u.Email, u.Role))
                        .ToListAsync();
    
        return users;
    }

    public async Task<UserDTO?> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if(user == null)
            return null;
        
        return new UserDTO(user.Id, user.Name, user.Email, user.Role);
    }

    public async Task<UserDTO?> LoginUser(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if(user == null || !_hasher.VerifyPassword(user.Password, password))
            return null;

        return new UserDTO(user.Id, user.Name, user.Email, user.Role);
    }

    public async Task<UserDTO?> UpdateUser(Guid id, UpdateUserDTO userUpdate)
    {
        var user = await _context.Users.FindAsync(id);
        if(user == null)
            return null;
        
        if(userUpdate.Email != null)
            user.UpdateEmail(userUpdate.Email);

        if(userUpdate.Name != null)
            user.UpdateName(userUpdate.Name);
        
        if(userUpdate.Role != null)
            user.UpdateRole(userUpdate.Role);

        user.UpdatedAt = userUpdate.UpdatedAt;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new UserDTO(user.Id, user.Name, user.Email, user.Role);
    }
}