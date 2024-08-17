using PracticeProject.Enums;
using System.ComponentModel.DataAnnotations;

namespace PracticeProject.DTOs
{
    public class GetTransactionsDTO
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<GetClientsDTO> Clients { get; set; } = new List<GetClientsDTO>();
        public ICollection<GetPaymentMethodsDTO> PaymentMethods { get; set; } = new List<GetPaymentMethodsDTO>();
    }
}
