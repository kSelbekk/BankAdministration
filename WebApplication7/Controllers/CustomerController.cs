using System;
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

            var viewModel = new CustomerCustomerProfileViewModel
            {
                Id = dbCustomer.CustomerId,
                Birthday = dbCustomer.Customer.Birthday,
                Gender = dbCustomer.Customer.Gender,
                City = dbCustomer.Customer.City,
                Country = dbCustomer.Customer.Country,
                CountryCode = dbCustomer.Customer.CountryCode,
                Emailaddress = dbCustomer.Customer.Emailaddress,
                Givenname = dbCustomer.Customer.Givenname,
                Surname = dbCustomer.Customer.Surname,
                NationalId = dbCustomer.Customer.NationalId,
                Streetaddress = dbCustomer.Customer.Streetaddress,
                TelephoneCountryCode = dbCustomer.Customer.Telephonecountrycode,
                Telephonenumber = dbCustomer.Customer.Telephonenumber,
                Zipcode = dbCustomer.Customer.Zipcode,
                TotalBalance = dbCustomer.Account.Balance,
                Account = dbCustomer.Account
            };
            return View(viewModel);
        }
    }
}