namespace Modules.Shipping.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Shipping.Application.Commands;
using Modules.Shipping.Application.Queries;
using Modules.Shipping.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShippingController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShippingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShipmentCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Shipment);
    }

    [HttpPost("ship")]
    public async Task<IActionResult> Ship([FromBody] ShipOrderCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Shipment);
    }

    [HttpPut("tracking")]
    public async Task<IActionResult> UpdateTracking([FromBody] UpdateTrackingCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Shipment);
    }

    [HttpDelete("{shipmentId}")]
    public async Task<IActionResult> Cancel(int shipmentId)
    {
        var result = await _mediator.Send(new CancelShipmentCommand(shipmentId));

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Shipment);
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrder(int orderId)
    {
        var result = await _mediator.Send(new GetShipmentByOrderQuery(orderId));

        if (!result.Success)
            return NotFound(new { result.Error });

        return Ok(result.Shipment);
    }
}