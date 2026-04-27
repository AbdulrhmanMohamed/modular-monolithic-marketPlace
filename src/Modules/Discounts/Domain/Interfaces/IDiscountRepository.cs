namespace Modules.Discounts.Domain.Interfaces;

using Modules.Discounts.Domain.Entities;

public interface IDiscountRepository
{
    Task<List<Discount>> GetActiveAsync();
    Task<Discount?> GetByCodeAsync(string code);
    Task<Discount?> GetByIdAsync(int id);
    Task<Discount> CreateAsync(Discount discount);
    Task<Discount?> UpdateAsync(Discount discount);
    Task<bool> DeleteAsync(int id);
}