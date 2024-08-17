using System.ComponentModel.DataAnnotations;

namespace PracticeProject.DTOs
{
    public class PaymentMethodDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be at least zero")]
        public decimal AvailableBalance { get; set; }
    }
}
