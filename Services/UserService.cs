using BankApi.Models;
using BankApi.Services;
using BankApi.Helpers;
using BankApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly AccountNumberGenerator _accountNumberGenerator;

    public UserService(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        AccountNumberGenerator accountNumberGenerator)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _accountNumberGenerator = accountNumberGenerator;
    }

    // Skapar en ny kund med ett konto
    public async Task<(int userId, string accountNumber, string accountType)> CreateCustomerAsync(
        string username, string password, string? accountType)
    {
        if (await _userRepository.UsernameExistsAsync(username)) // Kontrollera om användarnamnet redan finns
            throw new Exception("Username already exists");

        var user = new User
        {
            Username = username,
            PasswordHash = PasswordHasher.Hash(password),
            Role = "Customer"
        };

        await _userRepository.AddAsync(user);

        var account = new Account
        {
            UserId = user.Id,
            AccountNumber = _accountNumberGenerator.Generate(),
            Type = accountType ?? "Personkonto",
            Balance = 0
        };

        await _accountRepository.AddAsync(account);

        return (user.Id, account.AccountNumber, account.Type);
    }

    // Hämtar alla kunder med deras konton
    public async Task<List<UserDto>> GetAllCustomersAsync()
    {
        var users = await _userRepository.GetAllCustomersWithAccountsAsync();

        return users.Select(u => new UserDto // Mapperar User till UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Role = u.Role,
            Accounts = u.Accounts?.Select(a => new AccountDto
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                Type = a.Type,
                Balance = a.Balance
            }).ToList() ?? new List<AccountDto>()
        }).ToList();
    }
}
