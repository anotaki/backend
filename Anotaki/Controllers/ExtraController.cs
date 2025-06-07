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
    public class ExtraController(IExtraService extraService)
    {
        private readonly IExtraService _extraService = extraService;

        [HttpGet]
        public async Task<IActionResult> GetAllExtras()
        {
            try
            {
                var data = await _extraService.GetAllExtras();
                return ApiResponse.Create("Extras retrieved successfully.", StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to retrieve extras.", StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateExtra([FromBody] CreateExtraRequestDTO dto, [FromQuery] int productId)
        {
            try
            {
                var data = await _extraService.CreateExtra(dto, productId);
                return ApiResponse.Create("Extra created successfully.", StatusCodes.Status201Created, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to create extra.", StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("get-by")]
        public async Task<IActionResult> GetAllExtrasByProductId([FromQuery] int productId)
        {
            try
            {
                var data = await _extraService.GetAllExtrasByProductId(productId);
                return ApiResponse.Create("Extras retrieved by product successfully.", StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to retrieve extras by product.", StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExtra([FromQuery] int extraId)
        {
            try
            {
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
                var data = await _extraService.UpdateExtra(extra);
                return ApiResponse.Create("Extra Updated", StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to update extra.", StatusCodes.Status400BadRequest, ex);
            }
        }
    }
}
