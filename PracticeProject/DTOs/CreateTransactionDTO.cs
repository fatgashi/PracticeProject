using PracticeProject.Enums;
using System.ComponentModel.DataAnnotations;

namespace PracticeProject.DTOs
{
    public class CreateTransactionDTO
    {
        public Guid TransactionId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(3, ErrorMessage = "Currency code must be 3 characters long")]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency code must be a 3-letter ISO code")]
        public string Currency { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid ClientId { get; set; }
        public Guid PaymentMethodId { get; set; }


    }
}
