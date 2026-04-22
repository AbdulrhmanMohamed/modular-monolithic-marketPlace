namespace Modules.Payment.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Payment.Application.Commands;
using Modules.Payment.Application.Queries;
using Modules.Payment.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process")]
    public async Task<IActionResult> Process([FromBody] ProcessPaymentCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Payment);
    }

    [HttpPost("refund")]
    public async Task<IActionResult> Refund([FromBody] RefundPaymentCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Payment);
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrder(int orderId)
    {
        var result = await _mediator.Send(new GetPaymentByOrderQuery(orderId));

        if (!result.Success)
            return NotFound(new { result.Error });

        return Ok(result.Payment);
    }
}