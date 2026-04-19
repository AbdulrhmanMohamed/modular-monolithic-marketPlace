namespace Modules.Cart.Application.Commands;

using MediatR;
using Modules.Cart.Application.Results;

public record AddToCartCommand(
    int UserId,
    int ProductId,
    int Quantity = 1
) : IRequest<CartResult>;

public record UpdateCartItemCommand(
    int CartItemId,
    int Quantity
) : IRequest<CartResult>;

public record RemoveFromCartCommand(
    int CartItemId
) : IRequest<CartResult>;

public record ClearCartCommand(
    int UserId
) : IRequest<CartResult>;