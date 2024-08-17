using PracticeProject.DTOs;

namespace PracticeProject.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<CreatePaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO paymentMethodDTO);
        Task<IEnumerable<GetPaymentMethodsDTO>> GetPaymentMethods();

        Task<GetPaymentMethodsDTO> GetPaymentMethodByIdAsync(Guid paymentMethodId);
        Task<bool> UpdatePaymentMethodAsync(Guid paymentMethodId, PaymentMethodDTO paymentMethodDto);
        Task<bool> DeletePaymentMethodAsync(Guid paymentMethodId);
    }
}
