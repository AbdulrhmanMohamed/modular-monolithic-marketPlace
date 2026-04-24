namespace Modules.Reviews.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Reviews.Domain.Entities;
using Modules.Reviews.Domain.Interfaces;
using Modules.Reviews.Infrastructure.Data;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewsDbContext _context;

    public ReviewRepository(ReviewsDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetByProductIdAsync(int productId)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<Review> CreateAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<Review?> UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review is null) return false;
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return true;
    }
}