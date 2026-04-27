namespace Modules.Wishlist.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Wishlist.Application.Commands;
using Modules.Wishlist.Application.Queries;
using Modules.Wishlist.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WishlistController : ControllerBase
{
    private readonly IMediator _mediator;

    public WishlistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddToWishlistCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Wishlist);
    }

    [HttpDelete("{wishlistId}")]
    public async Task<IActionResult> Remove(int wishlistId)
    {
        var result = await _mediator.Send(new RemoveFromWishlistCommand(wishlistId));

        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _mediator.Send(new GetUserWishlistQuery(userId));

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Wishlists);
    }
}