namespace BankApi.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}
