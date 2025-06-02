using anotaki_api.DTOs.Requests.Address;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Address> CreateAddress(User user, CreateAddressRequestDTO addressDTO);
        Task UpdateUserAddress(User user, int addressId, UpdateAddressRequestDTO dto);
        Task<List<Address>> GetAllUserAddress(User user);
        Task DeleteUserAddress(User user, int addressId);
    }
}
