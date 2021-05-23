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

        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {
            var viewModel = new HomePageModel();

            var query = _bankServices.GetAllDispositionsFromDatabase();

            var totalCustomers = query
                .Select(c => c.Customer)
                .Count();

            var totalBalance = query
                .Where(a => a.Type == "OWNER")
                .Select(p => p.Account.Balance)
                .Sum();

            var totalAccounts = query
                .Where(a => a.Type == "OWNER")
                .Select(p => p.AccountId)
                .Count();

            viewModel.IndexInformation = new HomePageModel.HomeIndexViewModel
            {
                TotalBalance = totalBalance,
                TotalAccounts = totalAccounts,
                TotalCustomers = totalCustomers
            };

            viewModel.CountryInformation = new HomePageModel.InformationPerCountry
            {
                CustomersPerCountries = query
                    .ToLookup(i => i.Customer.Country)
                    .Select(m => new HomePageModel.InformationPerCountry.NumberOfCustomersPerCountry
                    {
                        Country = m.Key,
                        TotalBalance = m.Where(o => o.Type == "OWNER").Select(a => a.Account).Select(p => p.Balance).Sum(),
                        Accounts = m.Where(o => o.Type == "OWNER").Select(i => i.Account).Distinct().Count()
                    }).ToList()
            };

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