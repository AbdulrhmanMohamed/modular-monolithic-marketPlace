namespace Modules.Discounts.Application.Handlers;

using MediatR;
using Modules.Discounts.Application.Commands;
using Modules.Discounts.Application.Results;
using Modules.Discounts.Domain.Interfaces;

public class UpdateDiscountHandler : IRequestHandler<UpdateDiscountCommand, DiscountResult>
{
    private readonly IDiscountRepository _repository;

    public UpdateDiscountHandler(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiscountResult> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId);

        if (discount is null)
            return DiscountResult.Bad("Discount not found");

        discount.Description = request.Description;
        discount.DiscountPercent = request.DiscountPercent;
        discount.MinOrderAmount = request.MinOrderAmount;
        discount.ValidFrom = request.ValidFrom;
        discount.ValidUntil = request.ValidUntil;

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