using anotaki_api.Models;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }

    }
}
