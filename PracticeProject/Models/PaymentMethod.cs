using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Models
{
    public class PaymentMethod
    {
        [Key]
        public Guid PaymentMethodId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be at least zero")]
        public decimal AvailableBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
