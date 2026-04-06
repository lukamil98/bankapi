using System.Collections.Generic;
using System.Security.Principal;

namespace BankApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Admin" or "Customer"
        public ICollection<Account> Accounts { get; set; }
    }
}
