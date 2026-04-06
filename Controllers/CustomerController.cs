using BankApi.Data;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")] // Bara kunder kan komma åt dessa endpoints
    public class CustomerController : ControllerBase
    {
        private readonly BankDbContext _context;
        private readonly AccountNumberGenerator _accountNumberGenerator;

        public CustomerController(BankDbContext context, AccountNumberGenerator accountNumberGenerator)
        {
            _context = context;
            _accountNumberGenerator = accountNumberGenerator;
        }

        // GET: Alla användarens konton
        [HttpGet("Accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            var userId = GetUserIdFromToken();

            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    a.Id,
                    a.AccountNumber,
                    a.Type,
                    a.Balance
                })
                .ToListAsync();

            return Ok(accounts);
        }

        // GET: Transaktioner för ett specifikt konto
        [HttpGet("Account/{accountId}/Transactions")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var userId = GetUserIdFromToken();

            var account = await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);

            if (account == null)
                return NotFound("Account not found or does not belong to user");

            var transactions = account.Transactions
                .Select(t => new
                {
                    t.Id,
                    t.Amount,
                    t.Date,
                    t.Description
                });

            return Ok(transactions);
        }

        // POST: Skapa nytt konto
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            var userId = GetUserIdFromToken();

            var account = new Account
            {
                UserId = userId,
                AccountNumber = _accountNumberGenerator.Generate(),
                Type = request.Type ?? "Personkonto",
                Balance = 0
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Created("", new
            {
                account.Id,
                account.AccountNumber,
                account.Type,
                account.Balance
            });
        }

        // POST: Transfer av pengar mellan konton
        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransferRequest request)
        {
            var userId = GetUserIdFromToken();

            var fromAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.FromAccountId && a.UserId == userId);

            if (fromAccount == null)
                return NotFound("From account not found or does not belong to user");

            var toAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == request.ToAccountNumber);

            if (toAccount == null)
                return NotFound("To account not found");

            if (fromAccount.Balance < request.Amount)
                return BadRequest("Insufficient funds");

            // Uttag från eget konto
            fromAccount.Balance -= request.Amount;
            _context.Transactions.Add(new Transaction
            {
                AccountId = fromAccount.Id,
                Amount = -request.Amount,
                Date = DateTime.UtcNow,
                Description = $"Transfer to {toAccount.AccountNumber}"
            });

            // Lägg till på mottagarens konto
            toAccount.Balance += request.Amount;
            _context.Transactions.Add(new Transaction
            {
                AccountId = toAccount.Id,
                Amount = request.Amount,
                Date = DateTime.UtcNow,
                Description = $"Transfer from {fromAccount.AccountNumber}"
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                FromAccountBalance = fromAccount.Balance,
                ToAccountBalance = toAccount.Balance
            });
        }

        // Helper method
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new Exception("UserId not found in token");

            return int.Parse(userIdClaim.Value);
        }



        // DTOs for requests
        public class CreateAccountRequest
        {
            public string? Type { get; set; }
        }

        public class TransferRequest
        {
            public int FromAccountId { get; set; }
            public string ToAccountNumber { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
