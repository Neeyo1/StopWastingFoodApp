using API.DTOs.Product;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var products = await unitOfWork.ProductRepository.GetProductsAsync();
        
        return Ok(products);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int productId)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdWithDetailsAsync(productId);
        if (product == null) return NotFound();

        return Ok(mapper.Map<ProductDto>(product));
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductCreateDto productCreateDto)
    {
        var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync(productCreateDto.CategoryId);
        if (category == null)
            return BadRequest($"Category of id {productCreateDto.CategoryId} does not exist");

        var product = mapper.Map<Product>(productCreateDto);

        unitOfWork.ProductRepository.AddProduct(product);

        if (await unitOfWork.Complete()) return Ok(mapper.Map<ProductDto>(product));
        return BadRequest("Failed to create product");
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPut("{productId}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(ProductUpdateDto productUpdateDto, int productId)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdAsync(productId);
        if (product == null) return BadRequest("Failed to find product");

        if (productUpdateDto.Name != null) product.Name = productUpdateDto.Name;
        if (productUpdateDto.CategoryId != null)
        {
            var category = await unitOfWork.CategoryRepository.GetCategoryByIdAsync((int)productUpdateDto.CategoryId);
            if (category == null)
                return BadRequest($"Category of id {productUpdateDto.CategoryId} does not exist");
            
            product.CategoryId = (int)productUpdateDto.CategoryId;
        }

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to edit product");
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete("{productId}")]
    public async Task<ActionResult> DeleteProduct(int productId)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdAsync(productId);
        if (product == null) return BadRequest("Failed to find product");
        
        unitOfWork.ProductRepository.DeleteProduct(product);

        if (await unitOfWork.Complete()) return NoContent();
        return BadRequest("Failed to delete product");
    }
}
