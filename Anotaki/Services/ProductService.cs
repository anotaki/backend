using anotaki_api.Data;
using anotaki_api.DTOs.Requests.Product;
using anotaki_api.DTOs.Response;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Services
{
    public class ProductService (AppDbContext context) : IProductService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.Include(x => x.Extras).ToListAsync();
        }

        public async Task<Product> CreateProduct(CreateProductRequestDTO dto)
        {
            var newProduct = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = dto.ImageUrl,
            }; 

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<Product> UpdateProduct(Product updated)
        {
            var product = await _context.Products
                .Include(p => p.Extras)
                .FirstOrDefaultAsync(p => p.Id == updated.Id);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            product.Name = updated.Name;
            product.Price = updated.Price;
            product.Description = updated.Description;
            product.ImageUrl = updated.ImageUrl;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                throw new Exception("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductsByCategory>> ProductsFilterByCategory()
        {
            var categories = await _context.Categories.Include(c => c.Products).Where(c => c.Products.Count > 0).ToListAsync();

            return [.. categories.Select(c => new ProductsByCategory
            {
                Name = c.Name,
                Products = c.Products,
            })];
        }
    }
}
