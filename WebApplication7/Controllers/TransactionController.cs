using System;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class TransactionController : BaseController
    {
        public TransactionController(BankAppDataContext appDataContext, IBankServices bankServices)
            : base(appDataContext, bankServices)
        {
        }

        public IActionResult TransactionConfirmed()
        {
            return View();
        }

        public IActionResult DepositMoney()
        {
            var viewModel = new TransactionDepositMoneyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DepositMoney(TransactionDepositMoneyViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var operation = viewModel.Bank == null ? "Credit" : "Collection from Another Bank";

            _bankServices.DepositTransaction(viewModel.AccountId, "", viewModel.AmountToSend, operation, viewModel.Bank, viewModel.MessageForReceiver);

            return RedirectToAction("TransactionConfirmed");
        }

        public IActionResult WithdrawalMoney()
        {
            var viewModel = new TransactionWithdrawalMoneyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult WithdrawalMoney(TransactionWithdrawalMoneyViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            if (!_bankServices.CheckIfCustomerAccountBalanceIsValid(viewModel.AccountId, viewModel.AmountToSend))
            {
                ModelState.AddModelError("AmountToSend", "You don't have enough money");
                return View(viewModel);
            }

            var operation = "Withdrawal in Cash";

            _bankServices.WithdralTransaction(viewModel.AccountId, "", viewModel.AmountToSend, viewModel.MessageForSender, operation, "");

            return RedirectToAction("TransactionConfirmed");
        }

        public IActionResult SendMoney()
        {
            var viewModel = new TransactionSendMoneyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SendMoney(TransactionSendMoneyViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            if (!_bankServices.CheckIfCustomerAccountBalanceIsValid(viewModel.AccountId, viewModel.AmountToSend))
            {
                ModelState.AddModelError("AmountToSend", "You don't have enough money");
                return View(viewModel);
            }

            var receiverAccount = _appDataContext.Accounts.FirstOrDefault(i => i.AccountId == viewModel.ToAccountId);

            var operation = receiverAccount == null ? "Remittance to Another Bank" : "Withdrawal in cash";

            _bankServices.WithdralTransaction(viewModel.AccountId, viewModel.ToAccountId.ToString(), viewModel.AmountToSend, viewModel.MessageForSender, operation, viewModel.Bank);

            if (receiverAccount == null) return RedirectToAction("TransactionConfirmed");

            operation = "Credit in Cash";

            _bankServices.DepositTransaction(viewModel.ToAccountId, viewModel.AccountId.ToString(), viewModel.AmountToSend, operation, viewModel.Bank, viewModel.MessageForReceiver);

            return RedirectToAction("TransactionConfirmed");
        }

        //Remote
        public IActionResult ValidateExistingAccountId(int AccountId)
        {
            return _bankServices.GetSpecificAccountFromDatabase(AccountId) == null ? Json($"No account with {AccountId} Id found") : Json(true);
        }

        public IActionResult ValidateNoNegativeNumber(decimal AmountToSend)
        {
            if (AmountToSend > 0)
            {
                return Json(true);
            }

            return Json("No negative number");
        }
    }
}