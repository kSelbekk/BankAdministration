using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    public class AdminController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(BankAppDataContext appDataContext, IBankServices bankServices, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            : base(appDataContext, bankServices)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET
        public IActionResult EditUser(string id)
        {
            var dbUser = _userManager.Users.FirstOrDefault(i => i.Id == id);
            var r = _userManager.GetRolesAsync(dbUser).Result;
            var viewModel = new AdminEditUserViewModel
            {
                UserName = dbUser.UserName,
                Email = dbUser.Email,
                Id = dbUser.Id,
                IsInRole =
            }

            return View();
        }
    }
}