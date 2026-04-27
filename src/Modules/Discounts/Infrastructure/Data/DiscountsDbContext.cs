namespace Modules.Discounts.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Discounts.Domain.Entities;

public class DiscountsDbContext : DbContext
{
    public DiscountsDbContext(DbContextOptions<DiscountsDbContext> options) : base(options) { }

    public DbSet<Discount> Discounts => Set<Discount>();
}