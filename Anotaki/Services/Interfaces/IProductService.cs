using anotaki_api.DTOs.Requests.Product;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(ProductRequestDTO product);
        Task DeleteProduct(int id);
        Task<List<Product>> GetAllProducts();
        Task<Product> UpdateProduct(Product updated);
    }
}
