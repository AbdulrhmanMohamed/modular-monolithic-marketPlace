namespace Modules.Order.Application.Commands;

using MediatR;
using Modules.Order.Application.Results;

public record PlaceOrderCommand(
    int UserId,
    int? AddressId
) : IRequest<OrderResult>;

public record CancelOrderCommand(int OrderId) : IRequest<OrderResult>;

public record UpdateOrderStatusCommand(int OrderId, OrderStatus Status) : IRequest<OrderResult>;