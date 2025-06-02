using anotaki_api.DTOs.Requests.Address;
using anotaki_api.DTOs.Requests.User;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IUserService
    {

        /**************************************************
         * 
         *                  User
         * 
         **************************************************/
        Task<User> CreateUser(CreateUserRequestDTO userDTO);
        Task<User?> FindByCpf(string cpf);
        Task<User?> FindByEmail(string email);
        Task<User?> FindById(int id);


        /**************************************************
         * 
         *                  Address
         * 
         **************************************************/
        Task<Address> CreateAddress(User user, CreateAddressRequestDTO addressDTO);
        Task UpdateUserAddress(User user, int addressId, UpdateAddressRequestDTO dto);
        Task<List<Address>> GetAllUserAddress(User user);
        Task DeleteUserAddress(User user, int addressId);
    }
}
