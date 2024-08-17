using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.DTOs;
using PracticeProject.Models;
using PracticeProject.Services.Interfaces;

namespace PracticeProject.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        public readonly ApplicationDbContext _context;
        public readonly IMapper _mapper;

        public PaymentMethodService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreatePaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO paymentMethodDTO)
        {
            var paymentMethod = new PaymentMethod
            {
                PaymentMethodId = Guid.NewGuid(),
                Name = paymentMethodDTO.Name,
                AvailableBalance = paymentMethodDTO.AvailableBalance,
            };

            _context.PaymentMethods.Add(paymentMethod);
            await _context.SaveChangesAsync();

            var mappedPayment = _mapper.Map<CreatePaymentMethodDTO>(paymentMethod);

            return mappedPayment;
        }

        public async Task<bool> DeletePaymentMethodAsync(Guid paymentMethodId)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                return false;
            }

            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GetPaymentMethodsDTO> GetPaymentMethodByIdAsync(Guid paymentMethodId)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                return null; // Payment method not found
            }

            return _mapper.Map<GetPaymentMethodsDTO>(paymentMethod);
        }

        public async Task<IEnumerable<GetPaymentMethodsDTO>> GetPaymentMethods()
        {
            var paymentMehtods = await _context.PaymentMethods.ToListAsync();

            if (paymentMehtods.Count == 0)
            {
                return null;
            }

            var mappedPayments = _mapper.Map<IEnumerable<GetPaymentMethodsDTO>>(paymentMehtods);

            return mappedPayments;
        }

        public async Task<bool> UpdatePaymentMethodAsync(Guid paymentMethodId, PaymentMethodDTO paymentMethodDto)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                return false;
            }

            _mapper.Map(paymentMethodDto, paymentMethod); 
            _context.PaymentMethods.Update(paymentMethod);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
