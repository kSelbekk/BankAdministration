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

        public AdminController(BankAppDataContext appDataContext, IBankServices bankServices, UserManager<IdentityUser> userManager) : base(appDataContext, bankServices)
        {
            _userManager = userManager;
        }

        // GET
        public IActionResult EditUser(string id)
        {
            var dbUser = _userManager.Users.FirstOrDefault(i => i.Id == id);

            var viewModel = new AdminEditUserViewModel();

            return View();
        }

        public List<SelectListItem> GetAllUserRoles()
        {
            var roles = _userManager.
        }
    }
}