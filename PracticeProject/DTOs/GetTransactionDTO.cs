using PracticeProject.Enums;

namespace PracticeProject.DTOs
{
    public class GetTransactionDTO
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public GetClientsDTO Client { get; set; }
        public GetPaymentMethodsDTO PaymentMethod { get; set; }
    }
}
