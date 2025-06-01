namespace anotaki_api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public List<OrderProductItem> Items { get; set; } = [];
        
        public decimal TotalPrice { get; set; }
        public string Note { get; set; }
    }

    public enum OrderStatus
    {
        Pending,         // esperando a loja confirmar o pedido
        Preparing,       // preparando o pedido
        OnTheWay,        // pedido na rota de entrega
        Delivered,       // pedido entregue ao cliente
        Cancelled        // pedido cancelado
    }

    public class OrderProductItem
    {

        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<OrderExtraItem> ExtrasItems { get; set; } = [];

        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderExtraItem
    {
        public int id { get; set; }
        public int ExtraId { get; set; }
        public Extra Extra { get; set; }
        public int OrderProductItemId { get; set; }
        public OrderProductItem OrderProductItem { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
