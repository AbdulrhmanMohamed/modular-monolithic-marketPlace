namespace Modules.Wishlist.Application.Handlers;

using MediatR;
using Modules.Wishlist.Application.Commands;
using Modules.Wishlist.Domain.Interfaces;

public class RemoveFromWishlistHandler : IRequestHandler<RemoveFromWishlistCommand, bool>
{
    private readonly IWishlistRepository _repository;

    public RemoveFromWishlistHandler(IWishlistRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.WishlistId);
    }
}