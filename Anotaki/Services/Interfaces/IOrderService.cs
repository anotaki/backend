using anotaki_api.DTOs.Requests.Order;
using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> AddProductToOrder(Order order, User user, AddProductToOrderDTO dto);
        Task CheckoutOrder(Order order, CheckoutOrderDTO dto);
        Task<Order> GetCart(int userId);
        Task<Order?> GetOrderById(int cartId);
    }
}
