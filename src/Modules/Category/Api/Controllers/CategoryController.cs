namespace Modules.Category.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Category.Application.Commands;
using Modules.Category.Application.Queries;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoryController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));

        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Category);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Category?.Id }, result.Category);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryCommand command)
    {
        var updateCommand = new UpdateCategoryCommand(id, command.Name, command.Description, command.ImageUrl, command.ParentId);

        var result = await _mediator.Send(updateCommand);

        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Category);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));

        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }

    [HttpPatch("{id:int}/move")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MoveCategory(int id, [FromBody] MoveCategoryCommand command)
    {
        var moveCommand = new MoveCategoryCommand(id, command.NewParentId);
        var result = await _mediator.Send(moveCommand);

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Category);
    }
}