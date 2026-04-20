namespace Modules.Order.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Order.Application.Commands;
using Modules.Order.Application.Queries;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Order?.Id }, result.Order);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id));

        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Order);
    }

    [HttpGet("by-number/{orderNumber}")]
    public async Task<IActionResult> GetByNumber(string orderNumber)
    {
        var result = await _mediator.Send(new GetOrderByNumberQuery(orderNumber));

        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Order);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        var result = await _mediator.Send(new GetUserOrdersQuery(userId));

        if (result.Orders == null || !result.Orders.Any())
            return NotFound(new { message = "No orders found for this user" });

        return Ok(result.Orders);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var result = await _mediator.Send(new CancelOrderCommand(id));

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Order);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusCommand command)
    {
        var statusCommand = new UpdateOrderStatusCommand(id, command.Status);
        var result = await _mediator.Send(statusCommand);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Order);
    }
}