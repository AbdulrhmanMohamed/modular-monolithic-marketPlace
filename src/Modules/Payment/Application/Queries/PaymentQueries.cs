namespace Modules.Payment.Application.Queries;

using MediatR;
using Modules.Payment.Application.Results;

public record GetPaymentByOrderQuery(
    int OrderId
) : IRequest<PaymentResult>;

public record GetPaymentByIdQuery(
    int PaymentId
) : IRequest<PaymentResult>;