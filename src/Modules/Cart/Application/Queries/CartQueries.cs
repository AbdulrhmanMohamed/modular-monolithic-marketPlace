namespace Modules.Cart.Application.Queries;

using MediatR;
using Modules.Cart.Application.Results;

public record GetCartQuery(int UserId) : IRequest<CartResult>;

public record GetCartItemQuery(int CartItemId) : IRequest<CartResult>;