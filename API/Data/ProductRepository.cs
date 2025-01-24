using API.DTOs.Product;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ProductRepository(DataContext context, IMapper mapper) : IProductRepository
{
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        return await context.Products
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        return await context.Products
            .FindAsync(productId);
    }

    public async Task<Product?> GetProductByIdWithDetailsAsync(int productId)
    {
        return await context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == productId);
    }
}
