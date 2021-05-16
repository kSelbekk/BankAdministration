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
            var dbCustomer = _bankServices.GetSpecificCustomerInformation(id);
            if (dbCustomer == null) return RedirectToAction("Index", "Home");

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
                TotalBalance = dbCustomer.Dispositions.Select(a => a.Account.Balance).Sum(),

                Dispositions = dbCustomer.Dispositions.Select(n => new CustomerDispositionsViewModel
                {
                    CustomerId = n.CustomerId,
                    AccountId = n.AccountId,
                    DispositionId = n.DispositionId,
                    Type = n.Type,
                    Account = new CustomerAccountViewModel
                    {
                        AccountId = n.Account.AccountId,
                        Balance = n.Account.Balance,
                        Created = n.Account.Created,
                        Frequency = n.Account.Frequency
                    },
                }).ToList()
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
                        PersonalNumber = dbCustomer.NationalId
                    }).ToList(),

                q = q,
                Page = page,
                TotalPages = (int)Math.Ceiling(pageCount)
            };

            return View(viewModel);
        }

        public IActionResult TransactionsForCustomer(int id)
        {
            var query = _bankServices.GetAllTransactionsFromSpecificCustomer(id, 0, 15);

            var viewModel = new CustomerListTransactionsForCustomerViewModel
            {
                CustomerTransactions = query.Select(p =>
                    new CustomerListTransactionsForCustomerViewModel.CustomerTransaction
                    {
                        Balance = p.Balance,
                        Type = p.Type,
                        Account = p.Account,
                        Amount = p.Amount,
                        Bank = p.Bank,
                        Operation = p.Operation,
                        Symbol = p.Symbol,
                        TransactionDate = p.Date,
                        TransactionId = p.TransactionId
                    }).ToList(),
                AccountId = id
            };

            return View(viewModel);
        }

        public IActionResult GetTransactions(int id, int skip)
        {
            var viewModel = new CustomerListTransactionsForCustomerViewModel.TransactionsFromViewModel
            {
                CustomerTransactions = _bankServices.GetAllTransactionsFromSpecificCustomer(id, skip, 15).Select(
                    p => new CustomerListTransactionsForCustomerViewModel.CustomerTransaction
                    {
                        Balance = p.Balance,
                        Type = p.Type,
                        Account = p.Account,
                        Amount = p.Amount,
                        Bank = p.Bank,
                        Operation = p.Operation,
                        Symbol = p.Symbol,
                        TransactionDate = p.Date,
                        TransactionId = p.TransactionId
                    }).ToList()
            };

            return View(viewModel);
        }

        public IActionResult CreatNewCustomer()
        {
            var viewModel = new CustomerCreatNewCustomerViewModel();
            return View(viewModel);
        }
    }
}