using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WebApplication7.Models;
using WebApplication7.ViewModels;

namespace WebApplication7.Services
{
    public class CustomerRepository
    {
        private readonly BankAppDataContext _bankAppDataContext;

        public CustomerRepository(BankAppDataContext bankAppDataContext)
        {
            _bankAppDataContext = bankAppDataContext;
        }
        public void EditCustomer(Customers dbCustomer, CashierEditUserAccountsViewModel viewModel)
        {
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
        }
        public CashierEditUserAccountsViewModel EditCustomer(Customers dbCustomer, List<SelectListItem> selectListItems)
        {
            var viewModel = new CashierEditUserAccountsViewModel
            {
                Id = dbCustomer.CustomerId,
                City = dbCustomer.City,
                Country = dbCustomer.Country,
                CountryCode = dbCustomer.CountryCode,
                EmailAddress = dbCustomer.Emailaddress,
                Gender = dbCustomer.Gender,
                AllGenders = selectListItems,
                Givenname = dbCustomer.Givenname,
                NationalId = dbCustomer.NationalId,
                StreetAddress = dbCustomer.Streetaddress,
                Surname = dbCustomer.Surname,
                TelephoneCountryCode = dbCustomer.Telephonecountrycode,
                Telephonenumber = dbCustomer.Telephonenumber,
                Zipcode = dbCustomer.Zipcode
            };
            return viewModel;
        }
        public Customers CreatNewCustomer(CashierCreatNewCustomerViewModel viewModel)
        {
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

            _bankAppDataContext.Add(newCustomer);
            return newCustomer;
        }
        public void Update(Customers customers)
        {
            _bankAppDataContext.Customers.Update(customers);
        }
        public void SaveChanges()
        {
            _bankAppDataContext.SaveChanges();
        }
    }
}
