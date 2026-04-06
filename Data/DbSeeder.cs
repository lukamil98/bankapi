using BankApi.Helpers;
using BankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Data
{
    public static class DbSeeder
    {
        public static void SeedAdmin(BankDbContext context)
        {
            context.Database.Migrate(); // Makes sure base is created and all migrations are applied

            // Checks if there is any admin user
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = PasswordHasher.Hash("password"), // plain password is "password"
                    Role = "Admin"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
