using anotaki_api.Data;
using anotaki_api.DTOs.Requests.Extra;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Services
{
    public class ExtraService(AppDbContext context) : IExtraService
    {

        private readonly AppDbContext _context = context;

        public async Task<Extra?> FindById(int extraId)
        {
            var extra = await _context.Extras.FirstOrDefaultAsync(e => e.Id == extraId);
            return extra;
        }

        public async Task<List<Extra>> GetAllExtras()
        {
            return await _context.Extras.ToListAsync();
        }

        public async Task<Extra> CreateExtra(CreateExtraRequestDTO dto, int productId)
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

        public async Task<List<Extra>> GetAllExtrasByProductId(int productId)
        {
            List<Extra> extras = await _context.ProductExtras
                .Where(pe => pe.ProductId == productId)
                .Select(pe => pe.Extra)
                .ToListAsync();

            return extras;
        }

        public async Task DeleteExtra(int extraId)
        {
            Extra? extra = await FindById(extraId);
            if (extra == null)
                throw new Exception("Extra not found");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var related = await _context.ProductExtras
                    .Where(pe => pe.ExtraId == extraId)
                    .ToListAsync();

                _context.ProductExtras.RemoveRange(related);
                _context.Extras.Remove(extra);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
