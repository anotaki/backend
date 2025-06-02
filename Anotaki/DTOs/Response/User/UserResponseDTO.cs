using anotaki_api.Models;

namespace anotaki_api.DTOs.Response.User
{
    public class UserResponseDTO
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public UserAddressResponseDTO? DefaultAddress { get; set; }
    }
}
