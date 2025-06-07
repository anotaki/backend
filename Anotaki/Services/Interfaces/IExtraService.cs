using anotaki_api.DTOs.Requests.Extra;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IExtraService
    {
        Task<Extra> CreateExtra(CreateExtraRequestDTO dto, int productId);
        Task DeleteExtra(int extraId);
        Task<Extra?> FindById(int extraId);
        Task<List<Extra>> GetAllExtras();
        Task<List<Extra>> GetAllExtrasByProductId(int productId);
    }
}
