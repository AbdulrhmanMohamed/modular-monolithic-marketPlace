namespace Modules.Cart.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Cart.Application.Commands;
using Modules.Cart.Application.Queries;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCart([FromQuery] int userId)
    {
        var result = await _mediator.Send(new GetCartQuery(userId));

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(new
        {
            result.Cart,
            result.Items,
            result.ItemCount,
            result.Subtotal
        });
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result);
    }

    [HttpPut("items/{itemId:int}")]
    public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateCartItemCommand command)
    {
        var updateCommand = new UpdateCartItemCommand(itemId, command.Quantity);
        var result = await _mediator.Send(updateCommand);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result);
    }

    [HttpDelete("items/{itemId:int}")]
    public async Task<IActionResult> RemoveItem(int itemId)
    {
        var result = await _mediator.Send(new RemoveFromCartCommand(itemId));

        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart([FromQuery] int userId)
    {
        var result = await _mediator.Send(new ClearCartCommand(userId));

        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
}