namespace Modules.Cart.Application.Handlers;

using MediatR;
using Modules.Cart.Application.Commands;
using Modules.Cart.Application.Results;
using Modules.Cart.Domain.Interfaces;

public class UpdateCartItemHandler(ICartRepository _cartRepo)
    : IRequestHandler<UpdateCartItemCommand, CartResult>
{
    public async Task<CartResult> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var cartItem = _cartRepo.GetCartWithItems(0).Result?
            .Items.FirstOrDefault(i => i.Id == request.CartItemId);

        if (cartItem == null)
            return new() { Success = false, Error = "Cart item not found" };

        if (request.Quantity <= 0)
        {
            await _cartRepo.RemoveItem(request.CartItemId);
        }
        else
        {
            cartItem.Quantity = request.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            await _cartRepo.UpdateItem(cartItem);
        }

        return new() { Success = true };
    }
}

public class RemoveFromCartHandler(ICartRepository _cartRepo)
    : IRequestHandler<RemoveFromCartCommand, CartResult>
{
    public async Task<CartResult> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var result = await _cartRepo.RemoveItem(request.CartItemId);

        return result
            ? new() { Success = true }
            : new() { Success = false, Error = "Cart item not found" };
    }
}

public class ClearCartHandler(ICartRepository _cartRepo)
    : IRequestHandler<ClearCartCommand, CartResult>
{
    public async Task<CartResult> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepo.GetByUserId(request.UserId);
        if (cart == null)
            return new() { Success = false, Error = "Cart not found" };

        await _cartRepo.ClearCart(cart.Id);

        return new() { Success = true };
    }
}