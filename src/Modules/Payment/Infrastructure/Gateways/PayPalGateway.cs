namespace Modules.Payment.Infrastructure.Gateways;

using Modules.Payment.Domain.Interfaces;

public class PayPalGateway : IPaymentGateway
{
    public async Task<ChargeResult> Charge(decimal amount, string currency, string paymentMethod)
    {
        // Simulate PayPal API call
        await Task.Delay(100);

        var success = new Random().Next(0, 10) > 2;
        
        return new ChargeResult(
            Success: success,
            TransactionId: success ? $"paypal_{Guid.NewGuid()}" : null,
            ErrorMessage: success ? null : "PayPal payment failed"
        );
    }

    public async Task<RefundResult> Refund(string transactionId, decimal amount)
    {
        // Simulate PayPal refund API call
        await Task.Delay(100);

        var success = new Random().Next(0, 10) > 1;
        
        return new RefundResult(
            Success: success,
            ErrorMessage: success ? null : "PayPal refund failed"
        );
    }
}
