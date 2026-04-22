namespace Modules.Payment.Application.Commands;

using MediatR;
using Modules.Payment.Application.Results;

public record ProcessPaymentCommand(
    int OrderId,
    string Method,
    decimal Amount
) : IRequest<PaymentResult>;

public record RefundPaymentCommand(
    int PaymentId
) : IRequest<PaymentResult>;