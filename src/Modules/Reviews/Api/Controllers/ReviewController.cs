namespace Modules.Reviews.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Reviews.Application.Commands;
using Modules.Reviews.Application.Queries;
using Modules.Reviews.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Review);
    }

    [HttpPut("{reviewId}")]
    public async Task<IActionResult> Update(int reviewId, [FromBody] UpdateReviewCommand command)
    {
        var result = await _mediator.Send(command with { ReviewId = reviewId });

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Review);
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> Delete(int reviewId)
    {
        var result = await _mediator.Send(new DeleteReviewCommand(reviewId));

        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpPost("approve")]
    public async Task<IActionResult> Approve([FromBody] ApproveReviewCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Review);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetByProduct(int productId)
    {
        var result = await _mediator.Send(new GetProductReviewsQuery(productId));

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Reviews);
    }
}