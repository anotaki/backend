using anotaki_api.DTOs.Response.Api;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anotaki_api.Controllers
{
	[Route("api/v1/category")]
	[ApiController]
	[Authorize(Roles = Roles.Admin)]
	public class CategoryController(ICategoryService categoryService, IUserService userService) : ControllerBase
	{
		private readonly ICategoryService _categoryService = categoryService;
		private readonly IUserService _userService = userService;

        [HttpGet]
		public async Task<IActionResult> GetAllCategories()
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _categoryService.GetAllCategories();
				return ApiResponse.Create("Categories retrieved successfully.", StatusCodes.Status200OK, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to retrieve categories.", StatusCodes.Status400BadRequest, ex);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromQuery] string name)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _categoryService.CreateCategory(name);
				return ApiResponse.Create("Category created successfully.", StatusCodes.Status201Created, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to create category.", StatusCodes.Status400BadRequest, ex);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory([FromRoute] int id)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				await _categoryService.DeleteCategory(id);
				return ApiResponse.Create("Category delete successfully.", StatusCodes.Status200OK, id);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to delete category.", StatusCodes.Status400BadRequest, ex);
			}
		}

		[HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id,[FromQuery] string name)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _categoryService.UpdateCategory(name, id);
				return ApiResponse.Create("Category Updated", StatusCodes.Status200OK, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to update category.", StatusCodes.Status400BadRequest, ex);
			}
		}
    }
}
