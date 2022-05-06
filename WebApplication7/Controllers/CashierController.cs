using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    [Authorize(Roles = "Cashier, Admin")]
    public class CashierController : BaseController
    {
        private readonly CustomerRepository _customerRepository;
        private readonly AccountsRepository _accountsRepository;

        public CashierController(BankAppDataContext appDataContext, IBankServices bankServices, CustomerRepository customerRepository, AccountsRepository accountsRepository)
            : base(appDataContext, bankServices)
        {
            _customerRepository = customerRepository;
            _accountsRepository = accountsRepository;
        }

        public IActionResult EditCustomer(int id)
        {
            var dbCustomer = _bankServices.GetSpecificCustomerInformation(id);
            var viewModel = _customerRepository.EditCustomer(dbCustomer, GetAllGenders());
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditCustomer(CashierEditUserAccountsViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dbCustomer = _bankServices.GetSpecificCustomerInformation(viewModel.Id);
            _customerRepository.EditCustomer(dbCustomer, viewModel);
            _customerRepository.SaveChanges();

            return RedirectToAction("CustomerProfile", "Customer", new { id = dbCustomer.CustomerId });
        }

        public IActionResult CreatNewCustomer()
        {
            var viewModel = new CashierCreatNewCustomerViewModel { AllGenders = GetAllGenders() };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreatNewCustomer(CashierCreatNewCustomerViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var newCustomer = _customerRepository.CreatNewCustomer(viewModel);
            _customerRepository.SaveChanges();
            _customerRepository.Update(newCustomer);

            _accountsRepository.CreateNewAccount(newCustomer);
            _accountsRepository.SaveChanges();

            return RedirectToAction("CustomerProfile", "Customer", new { id = newCustomer.CustomerId });
        }

        private List<SelectListItem> GetAllGenders()
        {
            var list = new List<SelectListItem>
            {
                new() { Text = "male", Value = "male" },
                new() { Text = "female", Value = "female" },
                new() { Text = "other", Value = "other" }
            };
            return list;
        }
    }
}