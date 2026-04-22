namespace Modules.Payment.Domain.Interfaces;

public record ChargeResult(
    bool Success,
    string? TransactionId,
    string? ErrorMessage
);

public record RefundResult(
    bool Success,
    string? ErrorMessage
);

public interface IPaymentGateway
{
    Task<ChargeResult> Charge(decimal amount, string currency, string paymentMethod);
    Task<RefundResult> Refund(string transactionId, decimal amount);
}
