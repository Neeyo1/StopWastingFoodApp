using API.DTOs.Product;
using API.Entities;

namespace API.Interfaces;

public interface IProductRepository
{
    void AddProduct(Product product);
    void DeleteProduct(Product product);
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int productId);
    Task<Product?> GetProductByIdWithDetailsAsync(int productId);
}
