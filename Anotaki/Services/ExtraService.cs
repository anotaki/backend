using anotaki_api.Data;
using anotaki_api.DTOs.Requests.Extra;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public async Task<Extra> CreateExtra(CreateExtraRequestDTO dto)
        {

            var newExtra = new Extra()
            {
                Name = dto.Name,
                Price = dto.Price,
            };

            _context.Extras.Add(newExtra);
            await _context.SaveChangesAsync();
            return newExtra;

        }

        public async Task DeleteExtra(int extraId)
        {
            Extra? existingExtra = await FindById(extraId);
            if (existingExtra == null)
                throw new Exception("Extra not found");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var related = await _context.ProductExtras
                    .Where(pe => pe.ExtraId == extraId)
                    .ToListAsync();

                _context.ProductExtras.RemoveRange(related);
                _context.Extras.Remove(existingExtra);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Extra> UpdateExtra(Extra extra)
        {

            Extra? existingExtra = await FindById(extra.Id);

            if (existingExtra == null)
                throw new Exception("Extra not found");

            if (!string.IsNullOrWhiteSpace(extra.Name))
                existingExtra.Name = extra.Name;

            if (extra.Price != 0)
                existingExtra.Price = extra.Price;

            _context.Extras.Update(existingExtra);
            await _context.SaveChangesAsync();

            return existingExtra;
        }

        public async Task<List<ProductExtra>> AddMultipleExtrasToProduct(int productId, List<int> extraIdList)
        {
            if (extraIdList == null || !extraIdList.Any())
                throw new ArgumentException("A lista de extras está vazia.");

            List<ProductExtra> updatedProductExtras = [];

            foreach (int extraId in extraIdList)
            {
                var productExtra = await AddExtraToProduct(extraId, productId);
                if(productExtra != null)
                    updatedProductExtras.Add(productExtra);

            }

            return updatedProductExtras;
        }
        
        private async Task<ProductExtra?> AddExtraToProduct(int extraId, int productId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
                var extraExists = await _context.Extras.AnyAsync(e => e.Id == extraId);

                if (!productExists || !extraExists)
                {
                    throw new ArgumentException("Produto ou Extra inválido.");
                }

                var alreadyExists = await _context.ProductExtras
                    .AnyAsync(pe => pe.ProductId == productId && pe.ExtraId == extraId);

                if (alreadyExists)
                    throw new InvalidOperationException("Este extra já está associado a este produto.");

                var newProductExtra = new ProductExtra
                {
                    ProductId = productId,
                    ExtraId = extraId,
                };

                _context.ProductExtras.Add(newProductExtra);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newProductExtra;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
        }
    }
}
