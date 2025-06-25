using anotaki_api.Data;
using anotaki_api.DTOs.Requests.Address;
using anotaki_api.DTOs.Response;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Services
{
    public class CategoryService(AppDbContext context) : ICategoryService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<Category>> GetAllCategories() => await _context.Categories.ToListAsync();

        public async Task<Category> CreateCategory(string name)
        {
            var category = new Category
            {
                Name = name,
                CreatedAt = DateTime.Now,
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategory(string name, int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id) ?? throw new Exception("Category not found.");

            if (!String.IsNullOrEmpty(name))
                category.Name = name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id) ?? throw new Exception("Category not found.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}
