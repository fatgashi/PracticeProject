using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.DTOs;
using PracticeProject.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracticeProject.Controllers
{
    [Route("api/payment_methods")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        public readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod(CreatePaymentMethodDTO createPaymentMethod)
        {
            var paymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(createPaymentMethod);

            return Ok(paymentMethod);
        }

        [HttpGet]
        public async Task<IEnumerable<GetPaymentMethodsDTO>> GetPaymentMethods()
        {
            return await _paymentMethodService.GetPaymentMethods();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethod(Guid id)
    {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
            if (paymentMethod == null)
            {
                return NotFound(new { Message = "Payment method not found." });
            }

            return Ok(paymentMethod);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod(Guid id, [FromBody] PaymentMethodDTO paymentMethodDto)
        {
            var result = await _paymentMethodService.UpdatePaymentMethodAsync(id, paymentMethodDto);
            if (!result)
            {
                return NotFound(new { Message = "Payment method not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(Guid id)
    {
            var result = await _paymentMethodService.DeletePaymentMethodAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Payment method not found." });
            }

            return NoContent();
        }
    }
}
