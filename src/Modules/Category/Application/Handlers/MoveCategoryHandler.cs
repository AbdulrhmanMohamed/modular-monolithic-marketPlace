namespace Modules.Category.Application.Handlers;

using MediatR;
using Modules.Category.Application.Commands;
using Modules.Category.Application.Results;
using Modules.Category.Domain.Interfaces;

public class MoveCategoryHandler(ICategoryRepository _categoryRepo) : IRequestHandler<MoveCategoryCommand, CategoryResult>
{
    public async Task<CategoryResult> Handle(MoveCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetById(request.Id);
        if (category == null)
            return new() { Success = false, Error = "Category not found" };

        if (request.NewParentId.HasValue)
        {
            var parent = await _categoryRepo.GetById(request.NewParentId.Value);
            if (parent == null)
                return new() { Success = false, Error = "Parent category not found" };
        }

        var moved = await _categoryRepo.Move(request.Id, request.NewParentId);
        if (!moved)
            return new() { Success = false, Error = "Failed to move category" };

        return new() { Success = true };
    }
}