using BankApi.Data;
using BankApi.DTOs;
using BankApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BankDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(BankDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var hashedPassword = PasswordHasher.Hash(request.Password);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == hashedPassword);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = GenerateToken(user);

            return Ok(new { token });
        }

        private string GenerateToken(Models.User user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()), // lägger till användar-ID som NameId
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),  // username
        new Claim(ClaimTypes.Role, user.Role)                           // role
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
