namespace Modules.Payment.Domain.Entities;

using Shared.Abstractions;

public enum PaymentStatus
{
    Pending = 0,
    Processing = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}

public class Payment : BaseEntity
{
    public int OrderId { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public decimal Amount { get; set; }
    public string? Method { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? ProcessedAt { get; set; }
}