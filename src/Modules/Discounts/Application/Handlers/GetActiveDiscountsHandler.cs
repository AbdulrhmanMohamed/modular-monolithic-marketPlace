namespace Modules.Discounts.Application.Handlers;

using MediatR;
using Modules.Discounts.Application.Queries;
using Modules.Discounts.Application.Results;
using Modules.Discounts.Domain.Interfaces;

public class GetActiveDiscountsHandler : IRequestHandler<GetActiveDiscountsQuery, DiscountListResult>
{
    private readonly IDiscountRepository _repository;

    public GetActiveDiscountsHandler(IDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiscountListResult> Handle(GetActiveDiscountsQuery request, CancellationToken cancellationToken)
    {
        var discounts = await _repository.GetActiveAsync();

        var dtos = discounts.Select(d => new DiscountDto(
            d.Id,
            d.Code,
            d.Description,
            d.DiscountPercent,
            d.MinOrderAmount,
            d.ValidFrom,
            d.ValidUntil,
            d.IsActive
        )).ToList();

        return DiscountListResult.Ok(dtos);
    }
}