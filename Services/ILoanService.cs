public interface ILoanService
{
    Task<decimal> AddLoanAsync(int accountId, decimal amount);
}
