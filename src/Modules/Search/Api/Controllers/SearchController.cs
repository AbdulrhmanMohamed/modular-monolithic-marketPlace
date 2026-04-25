namespace Modules.Search.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Search.Application.Commands;
using Modules.Search.Application.Queries;
using Modules.Search.Application.Results;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new SearchProductsQuery(q, page, pageSize));

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Products);
    }

    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetHistory(int userId)
    {
        var result = await _mediator.Send(new GetSearchHistoryQuery(userId));

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Histories);
    }
}