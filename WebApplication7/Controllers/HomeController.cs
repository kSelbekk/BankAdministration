using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Data;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BankAppDataContext bankAppDataContext, IBankServices bankServices)
            : base(bankAppDataContext, bankServices)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index(string q)
        {
            var viewModel = new StartPageModel();

            var query = _bankServices.GetAllDispositionsFromDatabase();

            var totalCustomers = query
                .Select(c => c.Customer)
                .Count();

            var totalBalance = query
                .Select(p => p.Account.Balance)
                .Sum();

            var totalAccounts = query
                .Where(a => a.Type == "OWNER")
                .Select(p => p.AccountId)
                .Count();

            viewModel.HomeIndexViewModels = new StartPageModel.HomeIndexViewModel()
            {
                TotalAccounts = totalAccounts,
                TotalBalance = totalBalance,
                TotalCustomers = totalCustomers
            };

            if (!string.IsNullOrEmpty(q))
            {
                var customerQuery = _bankServices.GetAllCustomersFromDatabase()
                    .Where(c => q == null
                                || c.Surname.Contains(q)
                                || c.Givenname.Contains(q)
                                || c.City.Contains(q))
                    .Take(50);

                viewModel.ListCustomersViewModels = customerQuery.Select(c => new StartPageModel.ListCustomersViewModel
                {
                    Address = c.Streetaddress,
                    FullName = c.Givenname + " " + c.Surname,
                    City = c.City,
                    CustomerId = c.CustomerId,
                    PersonalNumber = c.Birthday
                }).ToList();

                viewModel.q = q;
            }

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}