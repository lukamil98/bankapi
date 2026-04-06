using BankApi.Data;
using BankApi.Models;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankDbContext _context;

    public TransactionRepository(BankDbContext context) // Konstruktor för att initiera databaskontexten
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction) // Lägg till en ny transaktion
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
}
