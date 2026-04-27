namespace Modules.Discounts.Application.Queries;

using MediatR;
using Modules.Discounts.Application.Results;

public record GetActiveDiscountsQuery : IRequest<DiscountListResult>;

public record ValidateDiscountQuery(
    string Code,
    decimal OrderAmount
) : IRequest<DiscountResult>;