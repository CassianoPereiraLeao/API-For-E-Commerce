namespace Project.src.Domain.Interfaces;

public interface IProductService
{
    public Task<bool> GetAllProducts(int? page, int? minPrice, int? maxPrice);
    public Task<bool> GetProductById(Guid Id);
    public Task<bool> CreateProduct(Guid Id);
    public Task<bool> UpdateProduct(Guid Id);
    public Task<bool> DeleteProduct(Guid Id);
}