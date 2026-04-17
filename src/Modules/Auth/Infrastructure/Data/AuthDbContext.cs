namespace Modules.Auth.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Auth.Domain.Entities;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
}