namespace Modules.Cart.Application.Handlers;

using MediatR;
using Modules.Cart.Application.Commands;
using Modules.Cart.Application.Results;
using Modules.Cart.Domain.Interfaces;
using Modules.Products.Domain.Interfaces;

public class AddToCartHandler(ICartRepository _cartRepo, Modules.Products.Domain.Interfaces.IProductRepository _productRepo)
    : IRequestHandler<AddToCartCommand, CartResult>
{
    public async Task<CartResult> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetByIdAsync(request.ProductId);
        if (product == null)
            return new() { Success = false, Error = "Product not found" };

        if (!await _productRepo.CheckStockAsync(request.ProductId, request.Quantity))
            return new() { Success = false, Error = "Insufficient stock" };

        var cart = await _cartRepo.GetCartWithItems(request.UserId);

        if (cart == null)
        {
            cart = new Domain.Entities.Cart
            {
                UserId = request.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            cart = await _cartRepo.Create(cart);
        }

        var cartItem = new Domain.Entities.CartItem
        {
            CartId = cart.Id,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        var existing = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existing != null)
        {
            existing.Quantity += request.Quantity;
            existing.UpdatedAt = DateTime.UtcNow;
            cartItem = await _cartRepo.UpdateItem(existing);
        }
        else
        {
            cartItem = await _cartRepo.AddItem(cartItem);
        }

        var updatedCart = await _cartRepo.GetCartWithItems(request.UserId);

        return new()
        {
            Success = true,
            Cart = MapToDto(updatedCart!),
            Items = updatedCart!.Items.Select(MapItemToDto).ToList(),
            ItemCount = updatedCart.Items.Sum(i => i.Quantity),
            Subtotal = updatedCart.Items.Sum(i => i.Quantity * (product.Price))
        };
    }

    private static CartDto MapToDto(Domain.Entities.Cart cart) => new()
    {
        Id = cart.Id,
        UserId = cart.UserId,
        ExpiresAt = cart.ExpiresAt
    };

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