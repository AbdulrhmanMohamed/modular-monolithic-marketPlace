namespace Modules.Payment.Application.Handlers;

using MediatR;
using Modules.Payment.Application.Commands;
using Modules.Payment.Application.Results;
using Modules.Payment.Domain.Entities;
using Modules.Payment.Domain.Interfaces;
using Modules.Payment.Application.Services;

public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, PaymentResult>
{
    private readonly IPaymentRepository _repository;
    private readonly PaymentGatewaySelector _gatewaySelector;

    public ProcessPaymentHandler(IPaymentRepository repository, PaymentGatewaySelector gatewaySelector)
    {
        _repository = repository;
        _gatewaySelector = gatewaySelector;
    }

    public async Task<PaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            OrderId = request.OrderId,
            Method = request.Method,
            Amount = request.Amount,
            Status = PaymentStatus.Processing
        };

        var created = await _repository.CreateAsync(payment);

        var gateway = _gatewaySelector.SelectGateway(request.Method);
        var result = await gateway.Charge(created.Amount, "USD", request.Method);

        if (result.Success)
        {
            created.Status = PaymentStatus.Completed;
            created.ProcessedAt = DateTime.UtcNow;
            created.TransactionId = result.TransactionId;
        }
        else
        {
            created.Status = PaymentStatus.Failed;
        }

        var updated = await _repository.UpdateAsync(created);

        var dto = new PaymentDto(
            updated!.Id,
            updated.OrderId,
            updated.Status,
            updated.Amount,
            updated.Method,
            updated.TransactionId,
            updated.ProcessedAt
        );

        return PaymentResult.Ok(dto);
    }
}