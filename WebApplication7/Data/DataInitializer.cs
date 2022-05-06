using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication7.Models;

namespace WebApplication7.Data
{
    public class DataInitializer
    {
        public void InitializeDatabase(BankAppDataContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            applicationDbContext.Database.Migrate();
            SeedData(applicationDbContext, userManager);
        }

        private static void SeedData(BankAppDataContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            SeedRoles(applicationDbContext);
            SeedUsers(userManager);
        }

        private static void SeedRoles(BankAppDataContext applicationDbContext)
        {
            AddNewRoleIfNotExists(applicationDbContext, "Admin");
            AddNewRoleIfNotExists(applicationDbContext, "Cashier");
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            AddToRoleIfNotExists(userManager, "stefan.holmberg@systementor.se", "Hejsan123#", new[] { "Admin" });
            AddToRoleIfNotExists(userManager, "stefan.holmberg@nackademin.se", "Hejsan123#", new[] { "Cashier" });
        }

        private static void AddToRoleIfNotExists(UserManager<IdentityUser> userManager, string userName, string password,
            string[] role)
        {
            if (userManager.FindByEmailAsync(userName).Result != null) return;

            var identityUser = new IdentityUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true,
                NormalizedEmail = userName.ToUpper()
            };

            var result = userManager.CreateAsync(identityUser, password).Result;
            var identityResult = userManager.AddToRolesAsync(identityUser, role).Result;
        }

        private static void AddNewRoleIfNotExists(BankAppDataContext context, string role)
        {
            if (context.Roles.Any(r => r.Name == role)) return;

            context.Roles.Add(new IdentityRole { Name = role, NormalizedName = role });
            context.SaveChanges();
        }
    }
}