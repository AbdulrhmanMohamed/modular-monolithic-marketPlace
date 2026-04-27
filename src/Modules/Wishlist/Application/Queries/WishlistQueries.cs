namespace Modules.Wishlist.Application.Queries;

using MediatR;
using Modules.Wishlist.Application.Results;

public record GetUserWishlistQuery(
    int UserId
) : IRequest<WishlistListResult>;