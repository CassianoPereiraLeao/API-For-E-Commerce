using Project.src.DTOs;
using Project.src.Infra.Entities;

namespace Project.src.Infra.Repository.Interfaces;

public interface IProductDTO
{
    public Task<List<UserDTO>> GetAllProduct(int page, int limit);
    public Task<UserDTO?> GetProductById(Guid id);
    public Task<bool> CreateProduct(User user);
    public Task<UserDTO?> UpdateProduct(string email, string password);
    public Task<bool> DeleteProduct(Guid id);
}