using anotaki_api.DTOs.Requests;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces


{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDTO userDTO);
        Task<User> FindByCpf(string cpf);
    }
}