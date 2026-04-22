namespace Modules.Payment.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Modules.Payment.Domain.Entities;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    public DbSet<Payment> Payments => Set<Payment>();
}