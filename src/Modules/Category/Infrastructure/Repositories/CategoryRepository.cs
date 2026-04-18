namespace Modules.Category.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Category.Domain.Entities;
using Modules.Category.Domain.Interfaces;
using Modules.Category.Infrastructure.Data;

public class CategoryRepository : ICategoryRepository
{
    private readonly CategoryDbContext _context;

    public CategoryRepository(CategoryDbContext context)
    {
        _context = context;
    }

    private DbSet<Category> Categories => _context.Categories;

    public async Task<Category?> GetById(int id)
    {
        return await Categories.FindAsync(id);
    }

    public async Task<List<Category>> GetAll()
    {
        return await Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<List<Category>> GetByParentId(int? parentId)
    {
        return await Categories
            .Where(c => c.ParentId == parentId && c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
    }

    public async Task<Category> Create(Category category)
    {
        Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> Delete(int id)
    {
        var category = await Categories.FindAsync(id);
        if (category == null) return false;

        category.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Move(int id, int? newParentId)
    {
        var category = await Categories.FindAsync(id);
        if (category == null) return false;

        if (newParentId.HasValue && newParentId == id)
            return false;

        category.ParentId = newParentId;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsBySlug(string slug)
    {
        return await Categories.AnyAsync(c => c.Slug == slug);
    }

    public async Task<bool> ExistsByName(string name, int? parentId)
    {
        return await Categories.AnyAsync(c => c.Name == name && c.ParentId == parentId);
    }
}