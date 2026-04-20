namespace Modules.Order.Application.Handlers;

using MediatR;
using Modules.Order.Application.Queries;
using Modules.Order.Application.Results;
using Modules.Order.Domain.Interfaces;

public class GetOrderByIdHandler(IOrderRepository _orderRepo) : IRequestHandler<GetOrderByIdQuery, OrderResult>
{
    public async Task<OrderResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepo.GetById(request.OrderId);
        if (order == null)
            return new() { Success = false, Error = "Order not found" };

        return new()
        {
            Success = true,
            Order = MapToDto(order)
        };
    }

    private static OrderDto MapToDto(Domain.Entities.Order order) => new()
    {
        Id = order.Id,
        OrderNumber = order.OrderNumber,
        Status = order.Status,
        TotalAmount = order.TotalAmount,
        PlacedAt = order.PlacedAt,
        Items = order.Items.Select(i => new OrderItemDto
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            TotalPrice = i.TotalPrice
        }).ToList()
    };
}

public class GetUserOrdersHandler(IOrderRepository _orderRepo) : IRequestHandler<GetUserOrdersQuery, OrderResult>
{
    public async Task<OrderResult> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepo.GetByUserId(request.UserId);

        return new()
        {
            Success = true,
            Orders = orders.Select(MapToDto).ToList()
        };
    }

    private static OrderDto MapToDto(Domain.Entities.Order order) => new()
    {
        Id = order.Id,
        OrderNumber = order.OrderNumber,
        Status = order.Status,
        TotalAmount = order.TotalAmount,
        PlacedAt = order.PlacedAt
    };
}