using BankApi.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Transaction
{
    public int Id { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }
}
