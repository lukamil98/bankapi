using BankApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoanService _loanService;

        public AdminController(IUserService userService,
                               ILoanService loanService)
        {
            _userService = userService;
            _loanService = loanService;
        }

        // CREATE CUSTOMER + ACCOUNT
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            try
            {
                var result = await _userService.CreateCustomerAsync(
                    request.Username,
                    request.Password,
                    request.AccountType);

                return Created("", new
                {
                    UserId = result.userId,
                    AccountNumber = result.accountNumber,
                    AccountType = result.accountType
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ADD LOAN
        [HttpPost("AddLoan")]
        public async Task<IActionResult> AddLoan(AddLoanRequest request)
        {
            try
            {
                var newBalance = await _loanService.AddLoanAsync(
                    request.AccountId,
                    request.Amount
                );

                return Ok(new
                {
                    Message = "Loan added successfully",
                    NewBalance = newBalance
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        // GET ALL CUSTOMERS
        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _userService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // DTOs

        public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? AccountType { get; set; }
    }

    public class AddLoanRequest
        {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        }
    }   

}
