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
            var depositAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.AccountId);

            viewModel.Operation = viewModel.Bank == null ? "Credit" : "Collection from Another Bank";

            var depositTransaction = new Transactions
            {
                AccountId = depositAccount.AccountId,
                Amount = viewModel.AmountToDeposit,
                Bank = viewModel.Bank,
                Balance = depositAccount.Balance + viewModel.AmountToDeposit,
                Date = DateTime.Now,
                Operation = viewModel.Operation,
                Type = "Credit",
                AccountNavigation = depositAccount,
                Symbol = viewModel.MessageForReceiver
            };

            depositAccount.Balance += viewModel.AmountToDeposit;

            _appDataContext.Add(depositTransaction);
            _appDataContext.SaveChanges();

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
            if (!_bankServices.CheckIfCustomerAccountBalanceIsValid(viewModel.AccountId, viewModel.AmountToSend))
            {
                ModelState.AddModelError("AmountToSend", "You don't have enough money");
                return View(viewModel);
            }
            if (!ModelState.IsValid) return View(viewModel);

            var withdrawlAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.AccountId);

            var Operation = "Withdrawal in Cash";

            _bankServices.WithdraTransaction(viewModel.AccountId, "", viewModel.AmountToSend, viewModel.MessageForSender, Operation, "");

            //var withdrawlTransaction = new Transactions
            //{
            //    AccountId = viewModel.AccountId,
            //    AccountNavigation = withdrawlAccount,
            //    Amount = viewModel.AmountToSend,
            //    Balance = withdrawlAccount.Balance - viewModel.AmountToSend * -1,
            //    Date = DateTime.Now,
            //    Operation = viewModel.Operation,
            //    Type = "Debit",
            //    Symbol = viewModel.MessageForSender,
            //    Account = ""
            //};
            //withdrawlAccount.Balance -= viewModel.AmountToSend;

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
            if (!_bankServices.CheckIfCustomerAccountBalanceIsValid(viewModel.AccountId, viewModel.AmountToSend))
            {
                ModelState.AddModelError("AmountToSend", "You don't have enough money");
                return View(viewModel);
            }

            if (!ModelState.IsValid) return View(viewModel);

            var senderAccount = _appDataContext.Accounts.First(i => i.AccountId == viewModel.AccountId);

            var receiverAccount = _appDataContext.Accounts.First(i => i.AccountId == viewModel.ToAccountId);

            var operation = receiverAccount == null ? "Remittance to Another Bank" : "Withdrawal in cash";

            _bankServices.WithdraTransaction(viewModel.AccountId, viewModel.ToAccountId.ToString(), viewModel.AmountToSend, viewModel.MessageForSender, operation, viewModel.Bank);

            //var senderTransaction = new Transactions
            //{
            //    AccountId = viewModel.AccountId,
            //    Bank = viewModel.Bank,
            //    Account = viewModel.ToAccountId.ToString(),
            //    Balance = senderAccount.Balance - viewModel.AmountToSend,
            //    Amount = viewModel.AmountToSend * -1,
            //    Type = "Debit",
            //    Date = DateTime.Now,
            //    Operation = viewModel.Operation,
            //    Symbol = viewModel.MessageForSender,
            //    AccountNavigation = senderAccount
            //};

            //senderAccount.Balance -= viewModel.AmountToSend;

            if (receiverAccount != null)
            {
                viewModel.Operation = "Credit in Cash";

                var receiverTransaction = new Transactions
                {
                    AccountId = viewModel.ToAccountId,
                    Bank = null,
                    Balance = receiverAccount.Balance + viewModel.AmountToSend,
                    Type = "Credit",
                    Date = DateTime.Now,
                    Account = viewModel.AccountId.ToString(),
                    Operation = viewModel.Operation,
                    Amount = viewModel.AmountToSend,
                    Symbol = viewModel.MessageForReceiver,
                    AccountNavigation = receiverAccount
                };
                receiverAccount.Balance += viewModel.AmountToSend;

                _appDataContext.Add(receiverTransaction);
            }

            _appDataContext.SaveChanges();

            return RedirectToAction("TransactionConfirmed");
        }

        //Remote
        public IActionResult ValidateExistingAccountId(int AccountId)
        {
            return _bankServices.GetSpecificAccountFromDatabase(AccountId) == null ? Json($"No account with {AccountId} Id found") : Json(true);
        }

        public IActionResult ValidateNoNegativeNumber(decimal AmountToSend)
        {
            return AmountToSend > 0 ? Json(true) : Json("The amount can't be a negative number");
        }
    }
}