namespace Modules.Search.Domain.Interfaces;

using Modules.Search.Domain.Entities;
using ProductEntity = Modules.Products.Domain.Entities.Product;

public interface ISearchRepository
{
    Task<List<SearchHistory>> GetByUserIdAsync(int userId);
    Task<SearchHistory> CreateAsync(SearchHistory history);
    Task<bool> DeleteAsync(int id);
    Task<List<ProductEntity>> SearchProductsAsync(string query);
}