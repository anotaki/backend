using anotaki_api.DTOs.Requests.Api;
using anotaki_api.DTOs.Requests.Product;
using anotaki_api.DTOs.Response;
using anotaki_api.DTOs.Response.Api;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(CreateProductRequestDTO product);
        Task DeleteProduct(int id);
        Task<List<Product>> GetAllProducts();
        Task<PaginatedDataResponse<Product>> GetPaginatedProducts(PaginationParams paginationParams);
        Task<List<ProductsByCategory>> ProductsFilterByCategory();
        Task<Product> UpdateProduct(Product updated);
    }
}
