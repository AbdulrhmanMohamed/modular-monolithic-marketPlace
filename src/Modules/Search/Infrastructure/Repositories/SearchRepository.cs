using Microsoft.EntityFrameworkCore;
using Modules.Search.Domain.Entities;
using Modules.Search.Domain.Interfaces;
using ProductEntity = Modules.Products.Domain.Entities.Product;

namespace Modules.Search.Infrastructure.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly DbContext _context;

    public SearchRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<List<SearchHistory>> GetByUserIdAsync(int userId)
    {
        return await _context.Set<SearchHistory>()
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.CreatedAt)
            .Take(20)
            .ToListAsync();
    }

    public async Task<SearchHistory> CreateAsync(SearchHistory history)
    {
        _context.Set<SearchHistory>().Add(history);
        await _context.SaveChangesAsync();
        return history;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var history = await _context.Set<SearchHistory>().FindAsync(id);
        if (history is null) return false;
        _context.Set<SearchHistory>().Remove(history);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProductEntity>> SearchProductsAsync(string query)
    {
        var lowerQuery = query.ToLower();
        return await _context.Set<ProductEntity>()
            .Where(p => p.Name.ToLower().Contains(lowerQuery) || 
                       (p.Description != null && p.Description.ToLower().Contains(lowerQuery)))
            .ToListAsync();
    }
}