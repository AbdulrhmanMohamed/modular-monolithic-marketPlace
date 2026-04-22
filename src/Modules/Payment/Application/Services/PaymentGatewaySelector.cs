namespace Modules.Payment.Application.Services;

using Modules.Payment.Domain.Interfaces;

public class PaymentGatewaySelector
{
    private readonly StripeGateway _stripeGateway;
    private readonly PayPalGateway _paypalGateway;

    public PaymentGatewaySelector(StripeGateway stripeGateway, PayPalGateway paypalGateway)
    {
        _stripeGateway = stripeGateway;
        _paypalGateway = paypalGateway;
    }

    public IPaymentGateway SelectGateway(string method)
    {
        return method.ToLower() switch
        {
            "paypal" => _paypalGateway,
            _ => _stripeGateway  // Default to Stripe
        };
    }
}
