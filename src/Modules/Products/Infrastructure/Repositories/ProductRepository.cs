namespace Modules.Products.Infrastructure.Repositories;

using Modules.Products.Domain.Entities;
using Modules.Products.Domain.Interfaces;
using Modules.Products.Infrastructure.Data;

public class ProductRepository(ProductsDbContext _context) : IProductRepository
{
    public Task<List<Product>> GetAllAsync(int page, int pageSize)
    {
        return Task.FromResult(_context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList());
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        return Task.FromResult(_context.Products.FirstOrDefault(p => p.Id == id));
    }

    public Task<Product> CreateAsync(Product product)
    {
        var maxId = _context.Products.Any() ? _context.Products.Max(p => p.Id) : 0;
        product.Id = maxId + 1;
        _context.Products.Add(product);
        return Task.FromResult(product);
    }

    public Task<Product?> UpdateAsync(Product product)
    {
        var existing = _context.Products.FirstOrDefault(p => p.Id == product.Id);
        if (existing is null)
            return Task.FromResult<Product?>(null);

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Description = product.Description;
        existing.Stock = product.Stock;

        return Task.FromResult<Product?>(existing);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product is null)
            return Task.FromResult(false);

        _context.Products.Remove(product);
        return Task.FromResult(true);
    }

    public Task<List<Product>> SearchAsync(string name)
    {
        return Task.FromResult(_context.Products
            .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }

    public Task<bool> CheckStockAsync(int productId, int quantity)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        return Task.FromResult(product != null && product.Stock >= quantity);
    }

    public Task<bool> DecrementStockAsync(int productId, int quantity)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product is null || product.Stock < quantity)
            return Task.FromResult(false);

        product.Stock -= quantity;
        return Task.FromResult(true);
    }
}