using BankApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task<bool> UsernameExistsAsync(string username);
    Task<List<User>> GetAllCustomersWithAccountsAsync();
}