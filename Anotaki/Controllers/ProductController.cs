using anotaki_api.DTOs.Requests.Product;
using anotaki_api.DTOs.Response.Api;
using anotaki_api.Models;
using anotaki_api.Services;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anotaki_api.Controllers
{
	[Route("api/v1/product")]
	[Authorize(Roles = Roles.Admin)]
	[ApiController]
	public class ProductController(IProductService productService, IUserService userService) : ControllerBase
	{
		private readonly IProductService _productService = productService;
		private readonly IUserService _userService = userService;

		[HttpPost]
		public async Task<IActionResult> CreateProduct(CreateProductRequestDTO dto)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _productService.CreateProduct(dto);
				return ApiResponse.Create("Product Created", StatusCodes.Status201Created, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to create Product", StatusCodes.Status400BadRequest, ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _productService.GetAllProducts();
				return ApiResponse.Create("Getting All Products", StatusCodes.Status200OK, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to Get All Products", StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateProduct([FromBody] Product product)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				var data = await _productService.UpdateProduct(product);
				return ApiResponse.Create("Product Updated", StatusCodes.Status200OK, data);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to Update Product", StatusCodes.Status400BadRequest, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct([FromRoute] int id)
		{
			try
			{
				var user = await _userService.GetContextUser(User);
				if (user == null)
					return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

				await _productService.DeleteProduct(id);
				return ApiResponse.Create("Product Deleted", StatusCodes.Status200OK, id);
			}
			catch (Exception ex)
			{
				return ApiResponse.Create("Failed to Delete Product", StatusCodes.Status400BadRequest, ex.Message);
			}
		}
	}
}
