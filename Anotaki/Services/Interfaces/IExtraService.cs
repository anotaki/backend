using anotaki_api.DTOs.Requests.Extra;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IExtraService
    {
        Task<Extra> CreateExtra(ExtraRequestDTO dto, int productId);
        Task<List<Extra>> GetAllExtras();
    }
}
