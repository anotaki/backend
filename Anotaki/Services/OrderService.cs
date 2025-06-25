using anotaki_api.Data;
using anotaki_api.DTOs.Requests.Order;
using anotaki_api.Models;
using anotaki_api.Queues.Publishers.Interfaces;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Services
{
	public class OrderService(AppDbContext context, IOrderPublisher orderPublisher) : IOrderService
	{
		private readonly AppDbContext _context = context;
		private readonly IOrderPublisher _orderPublisher = orderPublisher;

		public async Task<Order> GetCart(int userId)
		{
			var cart = await _context
				.Orders.Include(x => x.Items)
				.FirstOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == OrderStatus.Cart);

			if (cart == null)
			{
				var newCart = new Order
				{
					UserId = userId,
					OrderStatus = OrderStatus.Cart,
					TotalPrice = 0,
				};
				_context.Orders.Add(newCart);
				await _context.SaveChangesAsync();

				return newCart;
			}

			return cart;
		}

		public async Task<Order?> GetOrderById(int cartId)
		{
			return await _context.Orders.FindAsync(cartId);
		}

		public async Task CheckoutOrder(Order order, CheckoutOrderDTO dto)
		{
			//calculos e afins antes
			var address =
				await _context.Addresses.FindAsync(dto.AddressId) ?? throw new Exception($"Address with id {dto.AddressId} not found.");

			var totalPriceOrder = 0M;
			foreach (var item in order.Items)
			{
				totalPriceOrder += item.Quantity * item.TotalPrice;
			}

			order.AddressId = address.Id;
			order.TotalPrice = totalPriceOrder;
			order.OrderStatus = OrderStatus.Pending;
			order.Notes = dto.Notes;

			order.CreatedAt = DateTime.UtcNow;
			order.UpdatedAt = DateTime.UtcNow;

			_context.Update(order);
			await _context.SaveChangesAsync();

			//manda pra fila
			await _orderPublisher.Publish(order);
		}

		public async Task<Order> AddProductToOrder(Order order, User user, AddProductToOrderDTO dto)
		{
			var product = await _context.Products.FindAsync(dto.ProductId);

			if (product == null)
				throw new Exception($"Product with id {dto.ProductId} not found.");

			var productItem = new OrderProductItem()
			{
				OrderId = order.Id,
				ProductId = dto.ProductId,
				Quantity = 1,
				UnitPrice = product.Price,
				ExtrasItems = dto.Extras.Select(x => new OrderExtraItem() { ExtraId = x.ExtraId, Quantity = x.Quantity }).ToList(),
				Notes = dto.Notes,
			};

			var totalPriceProductItem = product.Price * productItem.Quantity;

			foreach (var item in productItem.ExtrasItems)
			{
				var extra = _context.Extras.Find(item.ExtraId);
				if (extra == null)
					throw new Exception($"Extra with id {item.ExtraId} not found.");

				totalPriceProductItem += item.Quantity * extra.Price;
			}

			productItem.TotalPrice = totalPriceProductItem;

			order.Items.Add(productItem);
			order.TotalPrice += productItem.TotalPrice;

			_context.Update(order);
			await _context.SaveChangesAsync();

			return order;
		}
	}
}
