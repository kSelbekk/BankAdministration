using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    [Authorize(Roles = "Admin")]
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

        public IActionResult Index()
        {
            var viewModel = new AdminIndexListViewModel
            {
                ListIndex = _userManager.Users.Select(p => new AdminIndexListViewModel.IndexViewModel
                {
                    UserName = p.UserName,
                    Id = p.Id,
                }).ToList(),
                UserRoles = _roleManager.Roles.ToList()
            };

            return View(viewModel);
        }

        public IActionResult EditUser(string id)
        {
            var dbUser = _userManager.Users.FirstOrDefault(i => i.Id == id);

            var viewModel = new AdminEditUserViewModel
            {
                UserName = dbUser.UserName,
                Email = dbUser.Email,
                Id = dbUser.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditUser(AdminEditUserViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var dbUser = _userManager.Users.First(i => viewModel.Id == i.Id);

            dbUser.UserName = viewModel.UserName;
            dbUser.Email = viewModel.Email;

            _appDataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.roleId = id;

            var role = await _roleManager.FindByIdAsync(id);

            var viewModel = new List<AdminEditUserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new AdminEditUserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                viewModel.Add(userRoleViewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<AdminEditUserRoleViewModel> viewModel, string roleId)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var role = await _roleManager.FindByIdAsync(roleId);

            for (var i = 0; i < viewModel.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(viewModel[i].UserId);

                IdentityResult result;

                if (viewModel[i].IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!viewModel[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (!result.Succeeded) continue;
                if (i < viewModel.Count - 1)
                    continue;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}