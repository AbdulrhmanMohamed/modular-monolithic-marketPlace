namespace Modules.Payment.Infrastructure.Gateways;

using Modules.Payment.Domain.Interfaces;

public class StripeGateway : IPaymentGateway
{
    public async Task<ChargeResult> Charge(decimal amount, string currency, string paymentMethod)
    {
        // Simulate Stripe API call
        await Task.Delay(100);

        var success = new Random().Next(0, 10) > 2;
        
        return new ChargeResult(
            Success: success,
            TransactionId: success ? $"stripe_{Guid.NewGuid()}" : null,
            ErrorMessage: success ? null : "Stripe payment failed"
        );
    }

    public async Task<RefundResult> Refund(string transactionId, decimal amount)
    {
        // Simulate Stripe refund API call
        await Task.Delay(100);

        var success = new Random().Next(0, 10) > 1;
        
        return new RefundResult(
            Success: success,
            ErrorMessage: success ? null : "Stripe refund failed"
        );
    }
}
