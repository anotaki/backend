using anotaki_api.Data;
using anotaki_api.DTOs.Requests.Extra;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace anotaki_api.Services
{
    public class ExtraService(AppDbContext context) : IExtraService
    {

        private readonly AppDbContext _context = context;

        public async Task<List<Extra>> GetAllExtras()
        {
            return await _context.Extras.ToListAsync();
        }

        public async Task<Extra> CreateExtra(ExtraRequestDTO dto, int productId)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newExtra = new Extra()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                };

                _context.Extras.Add(newExtra);
                await _context.SaveChangesAsync();

                var newProductExtra = new ProductExtra()
                {
                    ProductId = productId,
                    ExtraId = newExtra.Id,
                };

                _context.ProductExtras.Add(newProductExtra);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return newExtra;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
