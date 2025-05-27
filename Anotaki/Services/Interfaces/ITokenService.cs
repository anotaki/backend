using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
