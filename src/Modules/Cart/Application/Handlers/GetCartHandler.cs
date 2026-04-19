namespace Modules.Cart.Application.Handlers;

using MediatR;
using Modules.Cart.Application.Queries;
using Modules.Cart.Application.Results;
using Modules.Cart.Domain.Interfaces;

public class GetCartHandler(ICartRepository _cartRepo)
    : IRequestHandler<GetCartQuery, CartResult>
{
    public async Task<CartResult> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepo.GetCartWithItems(request.UserId);

        if (cart == null)
            return new() { Success = true, Items = new List<CartItemDto>(), ItemCount = 0 };

        return new()
        {
            Success = true,
            Cart = new CartDto { Id = cart.Id, UserId = cart.UserId, ExpiresAt = cart.ExpiresAt },
            Items = cart.Items.Select(MapItemToDto).ToList(),
            ItemCount = cart.Items.Sum(i => i.Quantity),
            Subtotal = cart.Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity)
        };
    }

    private static CartItemDto MapItemToDto(Domain.Entities.CartItem item) => new()
    {
        Id = item.Id,
        ProductId = item.ProductId,
        ProductName = item.Product?.Name ?? "Unknown",
        UnitPrice = item.Product?.Price ?? 0,
        Quantity = item.Quantity,
        TotalPrice = (item.Product?.Price ?? 0) * item.Quantity
    };
}