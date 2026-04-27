namespace Modules.Wishlist.Application.Results;

public record WishlistResult(
    bool Success,
    WishlistDto? Wishlist,
    string? Error
)
{
    public static WishlistResult Ok(WishlistDto wishlist) => new(true, wishlist, null);
    public static WishlistResult Bad(string error) => new(false, null, error);
};

public record WishlistListResult(
    bool Success,
    List<WishlistDto> Wishlists,
    string? Error
)
{
    public static WishlistListResult Ok(List<WishlistDto> wishlists) => new(true, wishlists, null);
    public static WishlistListResult Bad(string error) => new(false, new List<WishlistDto>(), error);
};

public record WishlistDto(
    int Id,
    int UserId,
    int ProductId
);