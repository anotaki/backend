using anotaki_api.DTOs.Requests.Extra;
using anotaki_api.DTOs.Response.Api;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anotaki_api.Controllers
{
	[Route("api/v1/extra")]
	[ApiController]
	[Authorize(Roles = Roles.Admin)]
	public class ExtraController(IExtraService extraService, IUserService userService) : ControllerBase
	{
		private readonly IExtraService _extraService = extraService;
		private readonly IUserService _userService = userService;

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetAllExtras()
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _extraService.GetAllExtras();
				return ApiResponse.Create("Extras retrieved successfully.", StatusCodes.Status200OK, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to retrieve extras.", StatusCodes.Status400BadRequest, ex);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateExtra([FromBody] CreateExtraRequestDTO dto)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _extraService.CreateExtra(dto);
				return ApiResponse.Create("Extra created successfully.", StatusCodes.Status201Created, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to create extra.", StatusCodes.Status400BadRequest, ex);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteExtra([FromQuery] int extraId)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				await _extraService.DeleteExtra(extraId);
				return ApiResponse.Create("Extra delete successfully.", StatusCodes.Status200OK, extraId);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to delete extra.", StatusCodes.Status400BadRequest, ex);
			}
		}

		[HttpPatch]
		public async Task<IActionResult> UpdateExtra([FromBody] Extra extra)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _extraService.UpdateExtra(extra);
				return ApiResponse.Create("Extra Updated", StatusCodes.Status200OK, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to update extra.", StatusCodes.Status400BadRequest, ex);
			}
		}

		[HttpPost]
		[Route("add-to-product/{productId}")]
		public async Task<IActionResult> AddMultipleExtrasToProduct([FromRoute] int productId, [FromBody] List<int> extraIds)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _extraService.AddMultipleExtrasToProduct(productId, extraIds);
				return ApiResponse.Create("Add Multiple Extras to Product", StatusCodes.Status200OK, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to add.", StatusCodes.Status400BadRequest, ex);
			}
		}
	}
}
