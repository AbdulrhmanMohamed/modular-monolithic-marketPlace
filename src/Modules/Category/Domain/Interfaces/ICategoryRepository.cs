namespace Modules.Category.Domain.Interfaces;

using Modules.Category.Domain.Entities;

public interface ICategoryRepository
{
    Task<Category?> GetById(int id);
    Task<List<Category>> GetAll();
    Task<List<Category>> GetByParentId(int? parentId);
    Task<Category> Create(Category category);
    Task<Category> Update(Category category);
    Task<bool> Delete(int id);
    Task<bool> Move(int id, int? newParentId);
    Task<bool> ExistsBySlug(string slug);
    Task<bool> ExistsByName(string name, int? parentId);
}