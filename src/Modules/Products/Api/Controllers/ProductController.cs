namespace Modules.Products.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Products.Application.Commands;
using Modules.Products.Application.Queries;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (pageSize > 100) pageSize = 100;

        var result = await _mediator.Send(new GetAllProductsQuery(page, pageSize));

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(new
        {
            result.Products,
            result.Page,
            result.PageSize,
            result.TotalCount
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));

        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Product?.Id }, result.Product);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateProductCommand command)
    {
        var updateCommand = new UpdateProductCommand(id, command.Name, command.Price, command.Description, command.Stock);

        var result = await _mediator.Send(updateCommand);

        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Product);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));

        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? name)
    {
        if (string.IsNullOrEmpty(name))
            return BadRequest(new { message = "Please provide a name parameter" });

        var result = await _mediator.Send(new SearchProductsQuery(name));

        if (!result.Success)
            return BadRequest(result.Error);

        return Ok(result.Products);
    }
}