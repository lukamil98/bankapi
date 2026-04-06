namespace Bank_Api.DTOs
{
    public class CreateLoanDto
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string LoanType { get; set; } = null!;
    }

}
