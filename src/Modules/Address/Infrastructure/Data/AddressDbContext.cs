namespace Modules.Address.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Address.Domain.Entities;

public class AddressDbContext : DbContext
{
    public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options) { }

    public DbSet<Address> Addresses => Set<Address>();
}