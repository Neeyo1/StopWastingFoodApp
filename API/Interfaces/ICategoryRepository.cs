using API.DTOs.Category;
using API.Entities;

namespace API.Interfaces;

public interface ICategoryRepository
{
    void AddCategory(Category category);
    void DeleteCategory(Category category);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int categoryId);
}
