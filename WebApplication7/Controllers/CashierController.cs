using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    //[Authorize(Roles = "Cashier")]
    public class CashierController :BaseController
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

            return RedirectToAction("CustomerProfile", "Customer", new {id = dbCustomer.CustomerId});
        }

    }
}