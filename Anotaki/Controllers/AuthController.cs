using anotaki_api.DTOs.Requests.Auth;
using anotaki_api.Models.Response;
using anotaki_api.Services.Interfaces;
using anotaki_api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace anotaki_api.Controllers
{
	[ApiController]
	[Route("api/v1/auth")]
	public class AuthController(ITokenService tokenService, IUserService userService) : ControllerBase
	{
		private readonly ITokenService _tokenService = tokenService;
		private readonly IUserService _userService = userService;

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
		{
			var user = await _userService.FindByEmail(loginDTO.Email);
			if (user == null || !HashUtils.VerifyPassword(loginDTO.Password, user.Password))
			{
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);
            }

            var token = _tokenService.CreateToken(user);
			return ApiResponse.Create("User auth successfully.", StatusCodes.Status200OK, new { token });
		}
	}
}
