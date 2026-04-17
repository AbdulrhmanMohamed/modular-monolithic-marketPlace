namespace Modules.Auth.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.Auth.Application.Commands;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator _mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Auth);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return Unauthorized(new { result.Error });

        return Ok(result.Auth);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return Unauthorized(new { result.Error });

        return Ok(result.Auth);
    }
}