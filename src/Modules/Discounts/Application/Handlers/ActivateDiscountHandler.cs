namespace Modules.Discounts.Application.Handlers;

using MediatR;
using Modules.Discounts.Application.Commands;
using Modules.Discounts.Application.Results;
using Modules.Discounts.Domain.Interfaces;

public class ActivateDiscountHandler : IRequestHandler<ActivateDiscountCommand, DiscountResult>
{
    private readonly IDiscountRepository _repository;

    public ActivateDiscountHandler(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiscountResult> Handle(ActivateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId);

        if (discount is null)
            return DiscountResult.Bad("Discount not found");

        discount.IsActive = true;

        var updated = await _repository.UpdateAsync(discount);

        var dto = new DiscountDto(
            updated!.Id,
            updated.Code,
            updated.Description,
            updated.DiscountPercent,
            updated.MinOrderAmount,
            updated.ValidFrom,
            updated.ValidUntil,
            updated.IsActive
        );

        return DiscountResult.Ok(dto);
    }
}