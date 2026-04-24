namespace Modules.Reviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Reviews.Domain.Entities;

public class ReviewsDbContext : DbContext
{
    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : base(options) { }

    public DbSet<Review> Reviews => Set<Review>();
}