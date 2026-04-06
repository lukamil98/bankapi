using BankApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserService
{
    Task<(int userId, string accountNumber, string accountType)> CreateCustomerAsync(
        string username,
        string password,
        string? accountType);

    Task<List<UserDto>> GetAllCustomersAsync();
}
