using System.ComponentModel.DataAnnotations;

namespace PracticeProject.DTOs
{
    public class CreatePaymentMethodDTO
    {
        public Guid PaymentMethodId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be at least zero")]
        public decimal AvailableBalance { get; set; }
    }
}
