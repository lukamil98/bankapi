using BankApi.Models;

public class LoanService : ILoanService
{
    private readonly IAccountRepository _accountRepo;
    private readonly ITransactionRepository _transactionRepo;

    public LoanService(IAccountRepository accountRepo,
                       ITransactionRepository transactionRepo)
    {
        _accountRepo = accountRepo;
        _transactionRepo = transactionRepo;
    }

    public async Task<decimal> AddLoanAsync(int accountId, decimal amount) // Lägg till lån till ett konto
    {
        var account = await _accountRepo.GetByIdAsync(accountId);

        if (account == null)
            throw new Exception("Account not found");

        account.Balance += amount;

        await _accountRepo.UpdateAsync(account);

        var transaction = new Transaction
        {
            AccountId = account.Id,
            Amount = amount,
            Date = DateTime.UtcNow,
            Description = "Loan deposit"
        };

        await _transactionRepo.AddAsync(transaction);

        return account.Balance;
    }
}
