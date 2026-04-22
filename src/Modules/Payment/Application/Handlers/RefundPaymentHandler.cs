namespace Modules.Payment.Application.Handlers;

using MediatR;
using Modules.Payment.Application.Commands;
using Modules.Payment.Application.Results;
using Modules.Payment.Domain.Entities;
using Modules.Payment.Domain.Interfaces;
using Modules.Payment.Application.Services;

public class RefundPaymentHandler : IRequestHandler<RefundPaymentCommand, PaymentResult>
{
    private readonly IPaymentRepository _repository;
    private readonly PaymentGatewaySelector _gatewaySelector;

    public RefundPaymentHandler(IPaymentRepository repository, PaymentGatewaySelector gatewaySelector)
    {
        _repository = repository;
        _gatewaySelector = gatewaySelector;
    }

    public async Task<PaymentResult> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.PaymentId);

        if (payment is null)
            return PaymentResult.Bad("Payment not found");

        if (!PaymentStateMachine.CanTransitionTo(payment.Status, PaymentStatus.Refunded))
            return PaymentResult.Bad($"Cannot transition from {payment.Status} to {PaymentStatus.Refunded}. Valid transitions: {string.Join(", ", PaymentStateMachine.GetValidTransitions(payment.Status))}");

        var gateway = _gatewaySelector.SelectGateway(payment.Method);
        var result = await gateway.Refund(payment.TransactionId!, payment.Amount);

        if (!result.Success)
            return PaymentResult.Bad(result.ErrorMessage ?? "Refund failed");

        payment.Status = PaymentStatus.Refunded;
        var updated = await _repository.UpdateAsync(payment);

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

    public async Task<PaymentResult> Handle(RefundPaymentHandler request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.PaymentId);

        if (payment is null)
            return PaymentResult.Bad("Payment not found");

        if (!PaymentStateMachine.CanTransitionTo(payment.Status, PaymentStatus.Refunded))
            return PaymentResult.Bad($"Cannot transition from {payment.Status} to {PaymentStatus.Refunded}. Valid transitions: {string.Join(", ", PaymentStateMachine.GetValidTransitions(payment.Status))}");

        payment.Status = PaymentStatus.Refunded;
        var updated = await _repository.UpdateAsync(payment);

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