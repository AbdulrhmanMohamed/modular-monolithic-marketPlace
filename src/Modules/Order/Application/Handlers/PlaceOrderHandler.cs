namespace Modules.Order.Application.Handlers;

using MediatR;
using Modules.Order.Application.Commands;
using Modules.Order.Application.Results;
using Modules.Order.Domain.Entities;
using Modules.Order.Domain.Interfaces;
using Modules.Cart.Domain.Interfaces;

public class PlaceOrderHandler(
    IOrderRepository _orderRepo,
    ICartRepository _cartRepo,
    Products.Domain.Interfaces.IProductRepository _productRepo
) : IRequestHandler<PlaceOrderCommand, OrderResult>
{
    public async Task<OrderResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepo.GetCartWithItems(request.UserId);
        if (cart == null || !cart.Items.Any())
            return new() { Success = false, Error = "Cart is empty" };

        foreach (var item in cart.Items)
        {
            if (!await _productRepo.CheckStockAsync(item.ProductId, item.Quantity))
                return new() { Success = false, Error = $"Insufficient stock for {item.Product?.Name}" };
        }

        var order = new Order
        {
            UserId = request.UserId,
            OrderNumber = GenerateOrderNumber(),
            Status = OrderStatus.Pending,
            Subtotal = cart.Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity),
            TaxAmount = cart.Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity) * 0.08m,
            ShippingAmount = 9.99m,
            DiscountAmount = 0,
            TotalAmount = 0,
            Currency = "USD",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        order.TotalAmount = order.Subtotal + order.TaxAmount + order.ShippingAmount - order.DiscountAmount;

        order = await _orderRepo.Create(order);

        foreach (var item in cart.Items)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product?.Price ?? 0,
                TotalPrice = (item.Product?.Price ?? 0) * item.Quantity
            };
            await _orderRepo.AddItem(orderItem);
            await _productRepo.DecrementStockAsync(item.ProductId, item.Quantity);
        }

        var payment = new Payment
        {
            OrderId = order.Id,
            Method = "Card",
            Amount = order.TotalAmount,
            Currency = "USD",
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        await _orderRepo.CreatePayment(payment);

        var shipment = new Shipment
        {
            OrderId = order.Id,
            AddressId = request.AddressId ?? 0,
            Carrier = "Standard",
            TrackingNumber = "",
            Status = ShipmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        await _orderRepo.CreateShipment(shipment);

        await _cartRepo.ClearCart(cart.Id);

        return new()
        {
            Success = true,
            Order = MapToDto(order)
        };
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }

    private static OrderDto MapToDto(Order order) => new()
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