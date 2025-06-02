using System.Security.Claims;
using anotaki_api.DTOs.Requests;
using anotaki_api.Models.Response;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Controllers
{
	[ApiController]
	[Route("/api/v1/users")]
	[Authorize]
	public class UserController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;

		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDTO)
		{
			var user = await _userService.CreateUser(userDTO);

			var data = new
			{
				name = user.Name,
				cpf = user.Cpf,
				email = user.Email,
			};

			return ApiResponse.Create("User created successfully.", StatusCodes.Status201Created, data);
		}

		[HttpPost]
		[Route("address")]
		public async Task<IActionResult> CreateAddress(CreateAddressDTO addressDTO)
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

			var user = await _userService.FindById(userId);

			if (user == null)
			{
				return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);
			}

			try
			{
				await _userService.CreateAddress(user, addressDTO);
			}
			catch (DbUpdateException ex)
			{
				return ApiResponse.Create(
					"Failed to save address.",
					StatusCodes.Status400BadRequest,
					new { message = ex.Message }
				);
			}

            return ApiResponse.Create("Address created successfully.", StatusCodes.Status200OK);
		}
	}
}
