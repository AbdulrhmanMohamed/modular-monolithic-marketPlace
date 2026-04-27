namespace Modules.Wishlist.Application.Handlers;

using MediatR;
using Modules.Wishlist.Application.Commands;
using Modules.Wishlist.Application.Results;
using Modules.Wishlist.Domain.Entities;
using Modules.Wishlist.Domain.Interfaces;

public class AddToWishlistHandler : IRequestHandler<AddToWishlistCommand, WishlistResult>
{
    private readonly IWishlistRepository _repository;

    public AddToWishlistHandler(IWishlistRepository repository)
    {
        _repository = repository;
    }

    public async Task<WishlistResult> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsAsync(request.UserId, request.ProductId))
            return WishlistResult.Bad("Product already in wishlist");

        var wishlist = new Wishlist
        {
            UserId = request.UserId,
            ProductId = request.ProductId
        };

        var created = await _repository.CreateAsync(wishlist);

        var dto = new WishlistDto(created.Id, created.UserId, created.ProductId);

        return WishlistResult.Ok(dto);
    }
}