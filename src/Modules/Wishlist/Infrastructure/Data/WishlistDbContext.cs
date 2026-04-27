namespace Modules.Wishlist.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Wishlist.Domain.Entities;

public class WishlistDbContext : DbContext
{
    public WishlistDbContext(DbContextOptions<WishlistDbContext> options) : base(options) { }

    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
}