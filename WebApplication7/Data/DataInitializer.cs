using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using WebApplication7.Models;

namespace WebApplication7.Data
{
    public class DataInitializer
    {
        public void InitializeDatabase(BankAppDataContext applicationDbContext)
        {
            SeedData(applicationDbContext);
            applicationDbContext.Database.Migrate();
        }

        public static void SeedData(BankAppDataContext applicationDbContext)
        {
            SeedRoles(applicationDbContext);
        }

        private static void SeedRoles(BankAppDataContext applicationDbContext)
        {
            var role = applicationDbContext.Roles.FirstOrDefault(a => a.Name == "Admin");
            if (role == null)
            {
                applicationDbContext.Roles.Add(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "admin".ToUpper()
                });
            }

            role = applicationDbContext.Roles.FirstOrDefault(a => a.Name == "Cashier");
            if (role == null)
            {
                applicationDbContext.Roles.Add(new IdentityRole
                {
                    Name = "Cashier",
                    NormalizedName = "Cashier".ToUpper()
                });
            }

            applicationDbContext.SaveChanges();
        }
    }
}