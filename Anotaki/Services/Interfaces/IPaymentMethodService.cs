using anotaki_api.Models;

namespace anotaki_api.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<PaymentMethod> CreatePaymentMethod(string name);
        Task<PaymentMethod> DeletePaymentMethod(int id);
        Task<List<PaymentMethod>> GetAllPaymentMethods();
        Task<PaymentMethod> UpdatePaymentMethod(string name, int id);
    }
}
