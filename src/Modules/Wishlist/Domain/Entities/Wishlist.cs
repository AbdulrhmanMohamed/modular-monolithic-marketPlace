namespace Modules.Wishlist.Domain.Entities;

using Shared.Abstractions;

public class Wishlist : BaseEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
}