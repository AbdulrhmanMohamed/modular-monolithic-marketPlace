namespace Modules.Category.Application.Handlers;

using MediatR;
using Modules.Category.Application.Commands;
using Modules.Category.Application.Results;
using Modules.Category.Domain.Interfaces;

public class DeleteCategoryHandler(ICategoryRepository _categoryRepo) : IRequestHandler<DeleteCategoryCommand, CategoryResult>
{
    public async Task<CategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepo.GetById(request.Id);
        if (category == null)
            return new() { Success = false, Error = "Category not found" };

        var hasChildren = await _categoryRepo.GetByParentId(category.Id);
        if (hasChildren.Count > 0)
            return new() { Success = false, Error = "Category has children, cannot delete" };

        await _categoryRepo.Delete(request.Id);

        return new() { Success = true };
    }
}