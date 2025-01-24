using API.DTOs.Category;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await unitOfWork.CategoryRepository.GetCategoriesAsync();
        
        return Ok(categories);
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int categoryId)
    {
        var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(categoryId);
        if (category == null) return NotFound();

        return Ok(mapper.Map<CategoryDto>(category));
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        var category = mapper.Map<Category>(categoryCreateDto);

        unitOfWork.CategoryRepository.AddCategory(category);

        if (await unitOfWork.Complete()) return Ok(mapper.Map<CategoryDto>(category));
        return BadRequest("Failed to create category");
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(CategoryCreateDto categoryUpdateDto, int categoryId)
    {
        var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(categoryId);
        if (category == null) return BadRequest("Failed to find category");

        mapper.Map(categoryUpdateDto, category);

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to edit category");
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete("{categoryId}")]
    public async Task<ActionResult> DeleteCategory(int categoryId)
    {
        var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(categoryId);
        if (category == null) return BadRequest("Failed to find category");
        
        unitOfWork.CategoryRepository.DeleteCategory(category);

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to delete category");
    }
}
