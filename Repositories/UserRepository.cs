using BankApi.Data;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly BankDbContext _context;

    public UserRepository(BankDbContext context) // Konstruktor för att initiera databaskontexten
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id) // Hämta användare efter ID
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetAllCustomersWithAccountsAsync()
    {
        return await _context.Users
            .Include(u => u.Accounts) // laddar in relaterade konton
            .ToListAsync();
    }


    public async Task AddAsync(User user) // Lägg till ny användare
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UsernameExistsAsync(string username) // Kontrollera om användarnamn redan finns
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}
