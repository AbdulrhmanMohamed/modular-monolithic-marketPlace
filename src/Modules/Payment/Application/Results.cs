namespace Modules.Payment.Application.Results;

public record PaymentResult(
    bool Success,
    PaymentDto? Payment,
    string? Error
)
{
    public static PaymentResult Ok(PaymentDto payment) => new(true, payment, null);
    public static PaymentResult Bad(string error) => new(false, null, error);
};

public record PaymentDto(
    int Id,
    int OrderId,
    PaymentStatus Status,
    decimal Amount,
    string? Method,
    string? TransactionId,
    DateTime? ProcessedAt
);