namespace Modules.Discounts.Application.Commands;

using MediatR;
using Modules.Discounts.Application.Results;

public record CreateDiscountCommand(
    string Code,
    string Description,
    decimal DiscountPercent,
    decimal? MinOrderAmount,
    DateTime ValidFrom,
    DateTime ValidUntil
) : IRequest<DiscountResult>;

public record UpdateDiscountCommand(
    int DiscountId,
    string Description,
    decimal DiscountPercent,
    decimal? MinOrderAmount,
    DateTime ValidFrom,
    DateTime ValidUntil
) : IRequest<DiscountResult>;

public record DeleteDiscountCommand(
    int DiscountId
) : IRequest<bool>;

public record ActivateDiscountCommand(
    int DiscountId
) : IRequest<DiscountResult>;

public record DeactivateDiscountCommand(
    int DiscountId
) : IRequest<DiscountResult>;