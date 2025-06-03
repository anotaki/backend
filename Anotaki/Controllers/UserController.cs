using anotaki_api.DTOs.Requests.User;
using anotaki_api.DTOs.Response.Api;
using anotaki_api.DTOs.Response.User;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using anotaki_api.Utils;
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO dto)
        {
            var user = await _userService.CreateUser(dto);

            var data = new UserResponseDTO
            {
                Name = user.Name,
                Cpf = user.Cpf,
                Email = user.Email
            };

            return ApiResponse.Create("User created successfully!", StatusCodes.Status201Created, data);

        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO dto)
        {
            var userId = ClaimUtils.GetUserId(User);
            if (userId == null)
            {
                return ApiResponse.Create("User not Authenticated.", StatusCodes.Status401Unauthorized);
            }

            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return ApiResponse.Create("User not Found.", StatusCodes.Status404NotFound);
            }

            try
            {
                await _userService.UpdateUser(dto, user);
                return ApiResponse.Create("User updated!", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create($"Error updating user: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

    }
}
