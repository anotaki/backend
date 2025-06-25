using anotaki_api.DTOs.Response.Api;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anotaki_api.Controllers
{
    [Route("api/v1/payment-method")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class PaymentMethodController(IPaymentMethodService paymentMethodService, IUserService userService) : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService = paymentMethodService;
        private readonly IUserService _userService = userService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaymentMethods()
        {
            try
            {
                var user = await _userService.GetContextUser(User);
                if (user == null)
                    return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

                var data = await _paymentMethodService.GetAllPaymentMethods();
                return ApiResponse.Create("Payment methods retrieved successfully.", StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to retrieve payment methods.", StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromQuery] string name)
        {
            try
            {
                var user = await _userService.GetContextUser(User);
                if (user == null)
                    return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

                var data = await _paymentMethodService.CreatePaymentMethod(name);
                return ApiResponse.Create("Payment method created successfully.", StatusCodes.Status201Created, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to create payment method.", StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod([FromRoute] int id)
        {
            try
            {
                var user = await _userService.GetContextUser(User);
                if (user == null)
                    return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

                await _paymentMethodService.DeletePaymentMethod(id);
                return ApiResponse.Create("Payment method deleted successfully.", StatusCodes.Status200OK, id);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to delete payment method.", StatusCodes.Status400BadRequest, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod([FromRoute] int id, [FromQuery] string name)
        {
            try
            {
                var user = await _userService.GetContextUser(User);
                if (user == null)
                    return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

                var data = await _paymentMethodService.UpdatePaymentMethod(name, id);
                return ApiResponse.Create("Payment method updated successfully.", StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to update payment method.", StatusCodes.Status400BadRequest, ex);
            }
        }
    }
}
