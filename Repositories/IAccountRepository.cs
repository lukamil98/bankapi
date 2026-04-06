using BankApi.Models;
using System.Threading.Tasks;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id);
    Task UpdateAsync(Account account);
    Task AddAsync(Account account);
}
