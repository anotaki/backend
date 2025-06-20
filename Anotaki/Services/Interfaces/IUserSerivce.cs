using anotaki_api.DTOs.Requests.Address;
using anotaki_api.DTOs.Requests.User;
using anotaki_api.Models;
using System.Security.Claims;

namespace anotaki_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDTO userDTO);
        Task<User?> FindByCpf(string cpf);
        Task<User?> FindByEmail(string email);
        Task<User?> FindById(int id);
        Task<User?> GetContextUser(ClaimsPrincipal user);
        Task<User> UpdateUser(UpdateUserDTO userDTO, User user);
    }
}
