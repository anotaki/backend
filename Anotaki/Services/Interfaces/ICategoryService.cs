using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(string name);
        Task<Category> DeleteCategory(int id);
        Task<List<Category>> GetAllCategories();
        Task<Category> UpdateCategory(string name, int id);
    }
}
