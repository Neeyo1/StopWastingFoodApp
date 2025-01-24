using API.DTOs.Category;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class CategoryRepository(DataContext context, IMapper mapper) : ICategoryRepository
{
    public void AddCategory(Category category)
    {
        context.Categories.Add(category);
    }

    public void DeleteCategory(Category category)
    {
        context.Categories.Remove(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        return await context.Categories
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        return await context.Categories
            .FindAsync(categoryId);
    }
}
