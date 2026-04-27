namespace Modules.Discounts.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Discounts.Application.Commands;
using Modules.Discounts.Application.Queries;
using Modules.Discounts.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDiscountCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Discount);
    }

    [HttpPut("{discountId}")]
    public async Task<IActionResult> Update(int discountId, [FromBody] UpdateDiscountCommand command)
    {
        var result = await _mediator.Send(command with { DiscountId = discountId });

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Discount);
    }

    [HttpDelete("{discountId}")]
    public async Task<IActionResult> Delete(int discountId)
    {
        var result = await _mediator.Send(new DeleteDiscountCommand(discountId));

        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _mediator.Send(new GetActiveDiscountsQuery());

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Discounts);
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] ValidateDiscountQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(new { result.Discount, result.DiscountAmount });
    }
}