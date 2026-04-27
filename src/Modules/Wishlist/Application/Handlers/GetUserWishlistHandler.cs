namespace Modules.Wishlist.Application.Handlers;

using MediatR;
using Modules.Wishlist.Application.Queries;
using Modules.Wishlist.Application.Results;
using Modules.Wishlist.Domain.Interfaces;

public class GetUserWishlistHandler : IRequestHandler<GetUserWishlistQuery, WishlistListResult>
{
    private readonly IWishlistRepository _repository;

    public GetUserWishlistHandler(IWishlistRepository repository)
    {
        _repository = repository;
    }

    public async Task<WishlistListResult> Handle(GetUserWishlistQuery request, CancellationToken cancellationToken)
    {
        var wishlists = await _repository.GetByUserIdAsync(request.UserId);

        var dtos = wishlists.Select(w => new WishlistDto(w.Id, w.UserId, w.ProductId)).ToList();

        return WishlistListResult.Ok(dtos);
    }
}