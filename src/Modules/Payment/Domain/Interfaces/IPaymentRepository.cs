namespace Modules.Payment.Domain.Interfaces;

using Modules.Payment.Domain.Entities;

public interface IPaymentRepository
{
    Task<List<Payment>> GetByOrderIdAsync(int orderId);
    Task<Payment?> GetByIdAsync(int id);
    Task<Payment> CreateAsync(Payment payment);
    Task<Payment?> UpdateAsync(Payment payment);
}