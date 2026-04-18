namespace Modules.Inventory.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Inventory.Application.Commands;
using Modules.Inventory.Application.Queries;
using Modules.Inventory.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetStock(int productId)
    {
        var result = await _mediator.Send(new GetStockQuery(productId));

        if (!result.Success)
            return NotFound(new { result.Error });

        return Ok(result.Inventory);
    }

    [HttpPost("decrement")]
    public async Task<IActionResult> Decrement([FromBody] DecrementStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Inventory);
    }

    [HttpPost("increment")]
    public async Task<IActionResult> Increment([FromBody] IncrementStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Inventory);
    }
}