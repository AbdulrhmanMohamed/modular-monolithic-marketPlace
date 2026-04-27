namespace Modules.Discounts.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Discounts.Domain.Entities;
using Modules.Discounts.Domain.Interfaces;
using Modules.Discounts.Infrastructure.Data;

public class DiscountRepository : IDiscountRepository
{
    private readonly DiscountsDbContext _context;

    public DiscountRepository(DiscountsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Discount>> GetActiveAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Discounts
            .Where(d => d.IsActive && d.ValidFrom <= now && d.ValidUntil >= now)
            .ToListAsync();
    }

    public async Task<Discount?> GetByCodeAsync(string code)
    {
        return await _context.Discounts.FirstOrDefaultAsync(d => d.Code == code.ToUpper());
    }

    public async Task<Discount?> GetByIdAsync(int id)
    {
        return await _context.Discounts.FindAsync(id);
    }

    public async Task<Discount> CreateAsync(Discount discount)
    {
        _context.Discounts.Add(discount);
        await _context.SaveChangesAsync();
        return discount;
    }

    public async Task<Discount?> UpdateAsync(Discount discount)
    {
        _context.Discounts.Update(discount);
        await _context.SaveChangesAsync();
        return discount;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var discount = await _context.Discounts.FindAsync(id);
        if (discount is null) return false;
        _context.Discounts.Remove(discount);
        await _context.SaveChangesAsync();
        return true;
    }
}