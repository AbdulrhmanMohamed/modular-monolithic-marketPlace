namespace Modules.Payment.Domain.Services;

using Modules.Payment.Domain.Entities;

public static class PaymentStateMachine
{
    private static readonly Dictionary<PaymentStatus, PaymentStatus[]> _validTransitions = new()
    {
        { PaymentStatus.Pending, new[] { PaymentStatus.Processing } },
        { PaymentStatus.Processing, new[] { PaymentStatus.Completed, PaymentStatus.Failed } },
        { PaymentStatus.Completed, new[] { PaymentStatus.Refunded } },
        { PaymentStatus.Failed, Array.Empty<PaymentStatus>() },
        { PaymentStatus.Refunded, Array.Empty<PaymentStatus>() }
    };

    public static bool CanTransitionTo(PaymentStatus currentStatus, PaymentStatus newStatus)
    {
        if (!_validTransitions.ContainsKey(currentStatus))
            return false;

        var allowedTransitions = _validTransitions[currentStatus];
        return allowedTransitions.Contains(newStatus);
    }

    public static PaymentStatus[] GetValidTransitions(PaymentStatus currentStatus)
    {
        if (!_validTransitions.ContainsKey(currentStatus))
            return Array.Empty<PaymentStatus>();

        return _validTransitions[currentStatus];
    }
}
