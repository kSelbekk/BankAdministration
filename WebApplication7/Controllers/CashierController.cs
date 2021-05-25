using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    [Authorize(Roles = "Cashier, Admin")]
    public class CashierController : BaseController
    {
        public CashierController(BankAppDataContext appDataContext, IBankServices bankServices)
            : base(appDataContext, bankServices)
        {
        }

        public IActionResult EditCustomer(int id)
        {
            var dbCustomer = _bankServices.GetSpecificCustomerInformation(id);

            var viewModel = new CashierEditUserAccountsViewModel
            {
                Id = dbCustomer.CustomerId,
                City = dbCustomer.City,
                Country = dbCustomer.Country,
                CountryCode = dbCustomer.CountryCode,
                EmailAddress = dbCustomer.Emailaddress,
                Gender = dbCustomer.Gender,
                Givenname = dbCustomer.Givenname,
                NationalId = dbCustomer.NationalId,
                StreetAddress = dbCustomer.Streetaddress,
                Surname = dbCustomer.Surname,
                TelephoneCountryCode = dbCustomer.Telephonecountrycode,
                Telephonenumber = dbCustomer.Telephonenumber,
                Zipcode = dbCustomer.Zipcode
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditCustomer(CashierEditUserAccountsViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var dbCustomer = _bankServices.GetSpecificCustomerInformation(viewModel.Id);

            dbCustomer.Givenname = viewModel.Givenname;
            dbCustomer.Country = viewModel.Country;
            dbCustomer.CountryCode = viewModel.CountryCode;
            dbCustomer.NationalId = viewModel.NationalId;
            dbCustomer.City = viewModel.City;
            dbCustomer.Surname = viewModel.Surname;
            dbCustomer.Gender = viewModel.Gender;
            dbCustomer.Telephonenumber = viewModel.Telephonenumber;
            dbCustomer.Telephonecountrycode = viewModel.TelephoneCountryCode;
            dbCustomer.Zipcode = viewModel.Zipcode;
            dbCustomer.Emailaddress = viewModel.EmailAddress;

            _appDataContext.SaveChanges();

            return RedirectToAction("CustomerProfile", "Customer", new { id = dbCustomer.CustomerId });
        }

        public IActionResult CreatNewCustomer()
        {
            var viewModel = new CashierCreatNewCustomerViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreatNewCustomer(CashierCreatNewCustomerViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var newCustomer = new Customers
            {
                Givenname = viewModel.Givenname,
                Surname = viewModel.Surname,
                Birthday = viewModel.Birthday,
                City = viewModel.City,
                Country = viewModel.Country,
                CountryCode = viewModel.CountryCode,
                Emailaddress = viewModel.Emailaddress,
                Gender = viewModel.Gender,
                NationalId = viewModel.NationalId,
                Streetaddress = viewModel.Streetaddress,
                Telephonecountrycode = viewModel.Telephonecountrycode,
                Telephonenumber = viewModel.Telephonenumber,
                Zipcode = viewModel.Zipcode,
                Dispositions = new List<Dispositions>()
            };

            _appDataContext.Add(newCustomer);
            _appDataContext.SaveChanges();

            _appDataContext.Customers.Update(newCustomer);

            var newAccount = new Accounts
            {
                Created = DateTime.Now,
                Frequency = "Monthly"
            };

            _appDataContext.Accounts.Update(newAccount);

            var newDisposition = new Dispositions
            {
                Account = newAccount,
                Customer = newCustomer,
                Type = "OWNER"
            };
            _appDataContext.Dispositions.Update(newDisposition);

            _appDataContext.SaveChanges();

            return View(viewModel);
        }
    }
}