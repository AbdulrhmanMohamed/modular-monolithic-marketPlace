namespace Modules.Wishlist.Application.Commands;

using MediatR;
using Modules.Wishlist.Application.Results;

public record AddToWishlistCommand(
    int UserId,
    int ProductId
) : IRequest<WishlistResult>;

public record RemoveFromWishlistCommand(
    int WishlistId
) : IRequest<bool>;