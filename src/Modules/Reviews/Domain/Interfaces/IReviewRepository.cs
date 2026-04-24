namespace Modules.Reviews.Domain.Interfaces;

using Modules.Reviews.Domain.Entities;

public interface IReviewRepository
{
    Task<List<Review>> GetByProductIdAsync(int productId);
    Task<Review?> GetByIdAsync(int id);
    Task<Review> CreateAsync(Review review);
    Task<Review?> UpdateAsync(Review review);
    Task<bool> DeleteAsync(int id);
}