namespace BankApi.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    }
}
