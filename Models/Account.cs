using System.ComponentModel.DataAnnotations.Schema;

namespace BankApi.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public string Type { get; set; } = "Personkonto";

        [Column(TypeName = "decimal(18,2)")] // Specificierar decimal precision och skala
        public decimal Balance { get; set; }

        // FK
        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
