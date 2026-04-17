namespace Modules.Products.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Products.Domain.Entities;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
}