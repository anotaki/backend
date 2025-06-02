using anotaki_api.DTOs.Requests.Address;
using anotaki_api.DTOs.Requests.User;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserRequestDTO userDTO);
        Task<User?> FindByCpf(string cpf);
        Task<User?> FindByEmail(string email);
        Task<User?> FindById(int id);
        Task<User> UpdateUser(UpdateUserRequestDTO userDTO, User user);
        Task DeactivateUser(User user);
        Task<User> ActivateUser(User user);
    }
}
