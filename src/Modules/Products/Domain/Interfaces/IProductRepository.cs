namespace Modules.Products.Domain.Interfaces;

using Modules.Products.Domain.Entities;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task<List<Product>> SearchAsync(string name);
    Task<bool> CheckStockAsync(int productId, int quantity);
    Task<bool> DecrementStockAsync(int productId, int quantity);
}