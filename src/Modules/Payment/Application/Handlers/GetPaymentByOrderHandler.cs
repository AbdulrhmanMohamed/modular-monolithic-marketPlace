namespace Modules.Payment.Application.Handlers;

using MediatR;
using Modules.Payment.Application.Queries;
using Modules.Payment.Application.Results;
using Modules.Payment.Domain.Interfaces;

public class GetPaymentByOrderHandler : IRequestHandler<GetPaymentByOrderQuery, PaymentResult>
{
    private readonly IPaymentRepository _repository;

    public GetPaymentByOrderHandler(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentResult> Handle(GetPaymentByOrderQuery request, CancellationToken cancellationToken)
    {
        var payments = await _repository.GetByOrderIdAsync(request.OrderId);
        var payment = payments.FirstOrDefault();

        if (payment is null)
            return PaymentResult.Bad("Payment not found");

        var dto = new PaymentDto(
            payment.Id,
            payment.OrderId,
            payment.Status,
            payment.Amount,
            payment.Method,
            payment.TransactionId,
            payment.ProcessedAt
        );

        return PaymentResult.Ok(dto);
    }
}