namespace Modules.Discounts.Application.Handlers;

using MediatR;
using Modules.Discounts.Application.Queries;
using Modules.Discounts.Application.Results;
using Modules.Discounts.Domain.Interfaces;

public class ValidateDiscountHandler : IRequestHandler<ValidateDiscountQuery, DiscountResult>
{
    private readonly IDiscountRepository _repository;

    public ValidateDiscountHandler(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiscountResult> Handle(ValidateDiscountQuery request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByCodeAsync(request.Code.ToUpper());

        if (discount is null)
            return DiscountResult.Bad("Invalid discount code");

        if (!discount.IsActive)
            return DiscountResult.Bad("Discount is not active");

        var now = DateTime.UtcNow;
        if (now < discount.ValidFrom || now > discount.ValidUntil)
            return DiscountResult.Bad("Discount has expired");

        if (discount.MinOrderAmount.HasValue && request.OrderAmount < discount.MinOrderAmount.Value)
            return DiscountResult.Bad($"Minimum order amount is {discount.MinOrderAmount.Value}");

        var discountAmount = request.OrderAmount * discount.DiscountPercent / 100;

        var dto = new DiscountDto(
            discount.Id,
            discount.Code,
            discount.Description,
            discount.DiscountPercent,
            discount.MinOrderAmount,
            discount.ValidFrom,
            discount.ValidUntil,
            discount.IsActive
        );

        return DiscountResult.Ok(dto, discountAmount);
    }
}