namespace Modules.Discounts.Application.Handlers;

using MediatR;
using Modules.Discounts.Application.Commands;
using Modules.Discounts.Application.Results;
using Modules.Discounts.Domain.Entities;
using Modules.Discounts.Domain.Interfaces;

public class CreateDiscountHandler : IRequestHandler<CreateDiscountCommand, DiscountResult>
{
    private readonly IDiscountRepository _repository;

    public CreateDiscountHandler(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiscountResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = new Discount
        {
            Code = request.Code.ToUpper(),
            Description = request.Description,
            DiscountPercent = request.DiscountPercent,
            MinOrderAmount = request.MinOrderAmount,
            ValidFrom = request.ValidFrom,
            ValidUntil = request.ValidUntil,
            IsActive = true
        };

        var created = await _repository.CreateAsync(discount);

        var dto = new DiscountDto(
            created.Id,
            created.Code,
            created.Description,
            created.DiscountPercent,
            created.MinOrderAmount,
            created.ValidFrom,
            created.ValidUntil,
            created.IsActive
        );

        return DiscountResult.Ok(dto);
    }
}