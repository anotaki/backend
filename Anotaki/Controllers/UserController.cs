using anotaki_api.DTOs.Requests.User;
using anotaki_api.DTOs.Response.User;
using anotaki_api.Models.Response;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static anotaki_api.Utils.ClaimUtils;

namespace anotaki_api.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {

        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO userDTO)
        {
            var user = await _userService.CreateUser(userDTO);

            var data = new UserResponseDTO
            {
                Name = user.Name,
                Cpf = user.Cpf,
                Email = user.Email
            };

            return ApiResponse.Create("User created successfully!", StatusCodes.Status201Created, data);

        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var userId = ClaimsUtils.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // mapeando endereço
            var defaultAddress = user.Addresses
                .Where(a => a.IsStandard)
                .Select(a => new UserAddressResponseDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    Number = a.Number,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    Neighborhood = a.Neighborhood,
                    Complement = a.Complement,
                    IsStandard = a.IsStandard
                })
                .FirstOrDefault();

            var data = new UserResponseDTO
            {
                Cpf = user.Cpf,
                Email = user.Email,
                Name = user.Name,
                DefaultAddress = defaultAddress
            };

            return ApiResponse.Create("Reading User Information", StatusCodes.Status200OK, data);
        }

    }
}
