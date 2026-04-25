namespace Modules.Notifications.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Notifications.Application.Commands;
using Modules.Notifications.Application.Queries;
using Modules.Notifications.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] SendNotificationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Notification);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _mediator.Send(new GetUserNotificationsQuery(userId));

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Notifications);
    }
}