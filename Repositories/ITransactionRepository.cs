using BankApi.Models;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
}
