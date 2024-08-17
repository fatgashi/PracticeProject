using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.DTOs;
using PracticeProject.Models;
using PracticeProject.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracticeProject.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<GetTransactionsDTO>> GetTransactions()
        {
            return await _transactionService.GetTransactions();
        }

        [HttpGet("client-transactions")]
        [Authorize]
        public async Task<IActionResult> GetClientTransactions()
    {
            // Extract the clientId from the token
            var clientIdClaim = User.FindFirst("clientId");

            if (clientIdClaim == null)
            {
                return Unauthorized("ClientId not found in token.");
            }

            if (!Guid.TryParse(clientIdClaim.Value, out var clientId))
            {
                return BadRequest("Invalid ClientId in token.");
            }

            var transactions = await _transactionService.GetTransactionsByClientIdAsync(clientId);

            if (transactions == null || !transactions.Any())
            {
                return NotFound("No transactions found.");
            }

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            if (transaction == null) { return NoContent(); }

            return Ok(transaction);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDTO createTransactionDTO)
        {
            var clientIdClaim = User.FindFirst("clientId");

            if (clientIdClaim == null)
            {
                return Unauthorized("ClientId not found in token.");
            }

            if (!Guid.TryParse(clientIdClaim.Value, out var clientId))
            {
                return BadRequest("Invalid ClientId in token.");
            }

            var transaction = await _transactionService.CreateTransaction(createTransactionDTO, clientId);

            if (transaction == null)
            { 
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveTransaction(Guid id)
    {
            var result = await _transactionService.ApproveTransactionAsync(id);

            if (!result)
            {
                return NotFound(new { Message = "Transaction not found or cannot be approved." });
            }

            return NoContent();
        }

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CancelTransaction(Guid id)
        {
            var result = await _transactionService.CancelTransactionAsync(id);

            if (!result)
            {
                return NotFound(new { Message = "Transaction not found or cannot be canceled." });
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] UpdateTransactionDTO updateTransactionDto)
        {
            var result = await _transactionService.UpdateTransactionAsync(id, updateTransactionDto);

            if (!result)
            {
                return NotFound(new { Message = "Transaction not found." });
            }

            return NoContent();
        }

    }
}
