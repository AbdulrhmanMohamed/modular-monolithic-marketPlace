namespace Modules.Address.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Address.Application.Commands;
using Modules.Address.Application.Queries;
using Modules.Address.Application.Results;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAddressCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Created("", result.Address);
    }

    [HttpPut("{addressId}")]
    public async Task<IActionResult> Update(int addressId, [FromBody] UpdateAddressCommand command)
    {
        var result = await _mediator.Send(command with { AddressId = addressId });

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Address);
    }

    [HttpDelete("{addressId}")]
    public async Task<IActionResult> Delete(int addressId)
    {
        var result = await _mediator.Send(new DeleteAddressCommand(addressId));

        if (!result)
            return NotFound();

        return Ok();
    }

    [HttpPost("default")]
    public async Task<IActionResult> SetDefault([FromBody] SetDefaultAddressCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Address);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var result = await _mediator.Send(new GetUserAddressesQuery(userId));

        if (!result.Success)
            return BadRequest(new { result.Error });

        return Ok(result.Addresses);
    }
}