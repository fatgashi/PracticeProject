using PracticeProject.Enums;

namespace PracticeProject.DTOs
{
    public class UpdateTransactionDTO
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
    }
}
