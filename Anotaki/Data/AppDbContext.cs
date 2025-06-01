using anotaki_api.Models;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Extra> Extras { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductExtra> ProductExtras { get; set; }
        public virtual DbSet<OrderProductItem> OrderProductItems { get; set; }
        public virtual DbSet<OrderExtraItem> OrderExtraItems { get; set; }
        
    }
}
