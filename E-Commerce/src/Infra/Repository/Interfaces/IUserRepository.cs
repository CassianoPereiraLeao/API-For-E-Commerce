using Project.src.DTOs;
using Project.src.Infra.Entities;
using Project.src.Routes.Request.Update;

namespace Project.src.Infra.Repository.Interfaces;

public interface IUserRepository
{
    public Task<List<UserDTO>> GetAllUsers(int page, int limit);
    public Task<UserDTO?> GetUserById(Guid id);
    public Task<bool> CreateUser(User user);
    public Task<UserDTO?> LoginUser(string email, string password);
    public Task<UserDTO?> UpdateUser(Guid id, UpdateUserDTO userUpdate);
    public Task<bool> DeleteUser(Guid id);
}