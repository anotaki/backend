using anotaki_api.DTOs.Requests.Order;
using anotaki_api.DTOs.Response;
using anotaki_api.DTOs.Response.Api;
using anotaki_api.Services.Interfaces;
using anotaki_api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace anotaki_api.Controllers
{
    [ApiController]
    [Route("/api/v1/orders")]
    [Authorize]
    public class OrderController(IOrderService orderService, IUserService userService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IUserService _userService = userService;

        [HttpGet("cart")]
        public async Task<IActionResult> GetCart()
        {
            var user = await _userService.GetContextUser(User);
            if (user == null)
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

            try
            {
                var cart = await _orderService.GetCart(user.Id);

                var data = new CartDTO
                {
                    Id = cart.Id,
                    OrderStatus = cart.OrderStatus,
                    TotalPrice = cart.TotalPrice,
                    Items = cart.Items,
                };

                return ApiResponse.Create("Successful.", StatusCodes.Status201Created, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed getting cart info." + ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("cart/{cartId}")]
        public async Task<IActionResult> AddProductToCart([FromRoute] int cartId, [FromBody] AddProductToOrderDTO dto)
        {
            var user = await _userService.GetContextUser(User);
            if (user == null)
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

            try
            {
                var cart = await _orderService.GetCart(user.Id);

                var updatedCart = await _orderService.AddProductToOrder(cart, user, dto);

                return ApiResponse.Create($"Added product id {dto.ProductId} successfully.", StatusCodes.Status201Created, updatedCart);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed adding product to cart.", StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("checkout-order/{orderId}")]
        public async Task<IActionResult> CheckoutOrder([FromRoute] int orderId, [FromBody] CheckoutOrderDTO dto)
        {
            var user = await _userService.GetContextUser(User);
            if (user == null)
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

            try
            {
                var cart = await _orderService.GetCart(user.Id);

                if (cart.UserId != user.Id)
                    return ApiResponse.Create("Order not belong to that user.", StatusCodes.Status404NotFound);

                await _orderService.CheckoutOrder(cart, dto);

                return ApiResponse.Create($"Order processed sucessfully.", StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed ordering.", StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
