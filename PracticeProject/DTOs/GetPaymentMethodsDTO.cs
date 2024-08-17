using System.ComponentModel.DataAnnotations;

namespace PracticeProject.DTOs
{
    public class GetPaymentMethodsDTO
    {
        public Guid PaymentMethodId { get; set; }
        public string Name { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}
