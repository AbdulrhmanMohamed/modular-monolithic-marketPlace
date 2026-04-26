namespace Modules.Media.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Media.Application.Commands;
using Modules.Media.Application.Queries;
using Modules.Media.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediaController : ControllerBase
{
    private readonly IMediator _mediator;

    public MediaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromBody] UploadMediaCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Media);
    }

    [HttpDelete("{mediaId}")]
    public async Task<IActionResult> Delete(int mediaId)
    {
        var result = await _mediator.Send(new DeleteMediaCommand(mediaId));

        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpGet("{mediaId}")]
    public async Task<IActionResult> GetById(int mediaId)
    {
        var result = await _mediator.Send(new GetMediaByIdQuery(mediaId));

        if (!result.Success)
            return NotFound(new { result.Error });

        return Ok(result.Media);
    }
}