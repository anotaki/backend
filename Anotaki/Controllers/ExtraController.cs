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
                return ApiResponse.Create("Getting all Extras", StatusCodes.Status200OK, data);
            } catch (Exception ex)
            {
                return ApiResponse.Create("Failed to Get All Extras", StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateExtra([FromBody] ExtraRequestDTO dto, [FromQuery] int productId)
        {
            try
            {
                var data = await _extraService.CreateExtra(dto, productId);
                return ApiResponse.Create("Extra adicionado", StatusCodes.Status201Created, data);
            }
            catch(Exception ex)
            {
                return ApiResponse.Create("Failed to create Extra", StatusCodes.Status400BadRequest, ex);
            }
        }
    }
}
