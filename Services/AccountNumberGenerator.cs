using BankApi.Data;
using System.Security.Cryptography;

namespace BankApi.Services
{
    public class AccountNumberGenerator
    {
        private readonly BankDbContext _context;

        public AccountNumberGenerator(BankDbContext context)
        {
            _context = context;
        }

        public string Generate()
        {
            string accountNumber;

            do
            {
                accountNumber = GenerateRandomNumber();
            }
            while (_context.Accounts.Any(a => a.AccountNumber == accountNumber));

            return accountNumber;
        }

        private string GenerateRandomNumber()
        {
            // Format: 8 digits (bank standard)
            using var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4];
            rng.GetBytes(bytes);

            int number = Math.Abs(BitConverter.ToInt32(bytes, 0));

            return (number % 100000000).ToString("D8");
        }
    }
}
