using BankApi.Data;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AccountRepository : IAccountRepository
{
    private readonly BankDbContext _context;

    public AccountRepository(BankDbContext context) // Konstruktor för att initiera databaskontexten
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(int id) // Hämta konto efter ID
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task UpdateAsync(Account account) // Uppdatera konto
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Account account) // Lägg till nytt konto
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
    }
}
