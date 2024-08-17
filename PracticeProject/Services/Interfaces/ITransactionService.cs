using PracticeProject.DTOs;

namespace PracticeProject.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<GetTransactionsDTO>> GetTransactions();
        Task<GetTransactionDTO> GetTransactionById(Guid id);
        Task<CreateTransactionDTO> CreateTransaction(CreateTransactionDTO transactionDTO, Guid id);
        Task<bool> ApproveTransactionAsync(Guid transactionId);
        Task<bool> CancelTransactionAsync(Guid transactionId);
        Task<bool> UpdateTransactionAsync(Guid transactionId, UpdateTransactionDTO updateTransactionDto);
        Task<IEnumerable<GetTransactionsDTO>> GetTransactionsByClientIdAsync(Guid clientId);
    }
}
