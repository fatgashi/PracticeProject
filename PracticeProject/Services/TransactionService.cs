using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Data;
using PracticeProject.DTOs;
using PracticeProject.Enums;
using PracticeProject.Models;
using PracticeProject.Services.Interfaces;

namespace PracticeProject.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApproveTransactionAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);

            if (transaction == null || transaction.Status != TransactionStatus.Pending)
            {
                return false;
            }

            transaction.Status = TransactionStatus.Approved;
            transaction.UpdatedAt = DateTime.UtcNow;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelTransactionAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);

            if (transaction == null || transaction.Status != TransactionStatus.Pending)
            {
                return false;
            }

            transaction.Status = TransactionStatus.Cancelled;
            transaction.UpdatedAt = DateTime.UtcNow;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<CreateTransactionDTO> CreateTransaction(CreateTransactionDTO transactionDTO, Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) { return null; }

            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(), 
                Amount = transactionDTO.Amount,
                Currency = transactionDTO.Currency,
                Status = transactionDTO.Status,
                Type = transactionDTO.Type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ClientId = id, 
                PaymentMethodId = transactionDTO.PaymentMethodId 
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var mappedTransaction = _mapper.Map<CreateTransactionDTO>(transaction);

            return mappedTransaction;
        }

        public async Task<GetTransactionDTO> GetTransactionById(Guid id)
        {
            var transaction = await _context.Transactions.Include(c => c.Client).ThenInclude(c => c.User).Include(p => p.PaymentMethod).FirstOrDefaultAsync(t => t.TransactionId == id);
            if (transaction == null) { return null; };

            var mappedTransaction = _mapper.Map<GetTransactionDTO>(transaction);

            return mappedTransaction;
        }

        public async Task<IEnumerable<GetTransactionsDTO>> GetTransactions()
        {
            var transactions = await _context.Transactions.Include(c => c.Client).ThenInclude(c => c.User).Include(p => p.PaymentMethod).ToListAsync();


            var mappedTransaction = _mapper.Map<IEnumerable<GetTransactionsDTO>>(transactions);

            return mappedTransaction;
        }

        public async Task<IEnumerable<GetTransactionsDTO>> GetTransactionsByClientIdAsync(Guid clientId)
        {
            var transactions = await _context.Transactions
            .Where(t => t.ClientId == clientId)
            .Include(t => t.Client)
            .ThenInclude(c => c.User)
            .Include(t => t.PaymentMethod)
            .ToListAsync();

            return _mapper.Map<IEnumerable<GetTransactionsDTO>>(transactions);
        }

        public async Task<bool> UpdateTransactionAsync(Guid transactionId, UpdateTransactionDTO updateTransactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);

            if (transaction == null)
            {
                return false; 
            }

            _mapper.Map(updateTransactionDto, transaction);

            transaction.UpdatedAt = DateTime.UtcNow;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
