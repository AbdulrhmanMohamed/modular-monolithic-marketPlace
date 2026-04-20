namespace Modules.Order.Application.Queries;

using MediatR;
using Modules.Order.Application.Results;

public record GetOrderByIdQuery(int OrderId) : IRequest<OrderResult>;

public record GetOrderByNumberQuery(string OrderNumber) : IRequest<OrderResult>;

public record GetUserOrdersQuery(int UserId) : IRequest<OrderResult>;