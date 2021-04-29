using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
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

            var query = _bankServices.GetBankAccountsFromCustomer(id);

            var totBalAccountsList = query.Select(a => a.Account.Balance).Sum();

            var account = query.Select(a => a.Account).ToList();

            var type = query.First(a => a.CustomerId == id);

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
                TotalBalance = totBalAccountsList,
                Type = type.Type
            };
            return View(viewModel);
        }

        public IActionResult ListCustomers(string q, int page = 1)
        {
            var customerQuery = _bankServices.GetAllCustomersFromDatabase()
                .Where(c => q == null
                            || c.Surname.Contains(q)
                            || c.Givenname.Contains(q)
                            || c.City.Contains(q));

            var totalRowCount = customerQuery.Count();

            var pageCount = (double)totalRowCount / 50;
            var howManyToSKip = (page - 1) * 50;
            customerQuery = customerQuery.Skip(howManyToSKip).Take(50);

            var viewModel = new CustomerListCustomersViewModel
            {
                CustomersViewModels = customerQuery.Select(dbCustomer =>
                    new CustomerListCustomersViewModel.ListCustomerViewModel
                    {
                        CustomerId = dbCustomer.CustomerId,
                        Address = dbCustomer.Streetaddress,
                        FullName = dbCustomer.Givenname + " " + dbCustomer.Surname,
                        City = dbCustomer.City,
                        PersonalNumber = dbCustomer.Birthday
                    }).ToList(),

                q = q,
                Page = page,
                TotalPages = (int)Math.Ceiling(pageCount)
            };

            return View(viewModel);
        }
    }
}