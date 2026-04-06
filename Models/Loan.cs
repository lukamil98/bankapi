using BankApi.Models;

public class Loan
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public string LoanType { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Account? Account { get; set; }
}
