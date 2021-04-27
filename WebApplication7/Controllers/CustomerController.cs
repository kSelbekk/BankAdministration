using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(BankAppDataContext appDataContext, IBankServices bankServices)
            : base(appDataContext, bankServices)
        {
        }

        // GET
        public IActionResult CustomerProfile(int id)
        {
            var dbCustomer = _bankServices.GetSpecificCustomerFromDatabase(id);
            if (dbCustomer == null) return View();

            var account = _bankServices.GetBankAccountsFromCustomer(id).ToList();
            var viewModel = new CustomerCustomerProfileViewModel
            {
                Id = dbCustomer.CustomerId,
                Birthday = dbCustomer.Birthday,
                Gender = dbCustomer.Gender,
                City = dbCustomer.City,
                Country = dbCustomer.Country,
                CountryCode = dbCustomer.CountryCode,
                Emailaddress = dbCustomer.Emailaddress,
                Givenname = dbCustomer.Givenname,
                Surname = dbCustomer.Surname,
                NationalId = dbCustomer.NationalId,
                Streetaddress = dbCustomer.Streetaddress,
                TelephoneCountryCode = dbCustomer.Telephonecountrycode,
                Telephonenumber = dbCustomer.Telephonenumber,
                Zipcode = dbCustomer.Zipcode,
                Account = account,
                TotalBalance = account.Sum(a => a.Balance)
            };
            return View(viewModel);
        }
    }
}