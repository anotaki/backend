using anotaki_api.Data;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Services
{
    public class PaymentMethodService(AppDbContext context) : IPaymentMethodService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<PaymentMethod>> GetAllPaymentMethods() => await _context.PaymentMethods.ToListAsync();

        public async Task<PaymentMethod> CreatePaymentMethod(string name)
        {
            var paymentMethod = new PaymentMethod
            {
                Name = name,
                CreatedAt = DateTime.Now,
            };

            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return paymentMethod;
        }

        public async Task<PaymentMethod> UpdatePaymentMethod(string name, int id)
        {
            var paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.Id == id) ?? throw new Exception("PaymentMethod not found.");

            if (!String.IsNullOrEmpty(name))
                paymentMethod.Name = name;

            _context.PaymentMethods.Update(paymentMethod);
            await _context.SaveChangesAsync();

            return paymentMethod;
        }

        public async Task<PaymentMethod> DeletePaymentMethod(int id)
        {
            var paymentMethod = _context.PaymentMethods.FirstOrDefault(c => c.Id == id) ?? throw new Exception("PaymentMethod not found.");

            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return paymentMethod;
        }
    }
}
