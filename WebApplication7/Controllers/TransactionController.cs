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
        private readonly SignInManager<IdentityUser> _userManager;

        public TransactionController(BankAppDataContext appDataContext, IBankServices bankServices, SignInManager<IdentityUser> userManager)
            : base(appDataContext, bankServices)
        {
            _userManager = userManager;
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
                AccountId = viewModel.AccountId,
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
            if (!ModelState.IsValid) return View(viewModel);

            var withdrawlAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.AccountId);

            if (!_bankServices.CheckIfCustomerAccountBalanceIsValid(withdrawlAccount.AccountId, viewModel.AmountToWithdrawal))
            {
                ModelState.AddModelError("AmountToWithdrawal", "You don't have enough money");
                return View(viewModel);
            }

            viewModel.Operation = _userManager.IsSignedIn(User) ? "Credit Card Withdrawal" : "Withdrawal in Cash";

            var withdrawlTransaction = new Transactions
            {
                AccountId = viewModel.AccountId,
                AccountNavigation = withdrawlAccount,
                Amount = viewModel.AmountToWithdrawal,
                Balance = withdrawlAccount.Balance - viewModel.AmountToWithdrawal * -1,
                Date = DateTime.Now,
                Operation = viewModel.Operation,
                Type = "Debit",
                Symbol = viewModel.MessageForSender
            };
            withdrawlAccount.Balance -= viewModel.AmountToWithdrawal;

            _appDataContext.Add(withdrawlTransaction);
            _appDataContext.SaveChanges();

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

            var senderAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.AccountId);

            var receiverAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.ToAccountId);

            viewModel.Operation = receiverAccount == null ? "Remittance to Another Bank" : "Withdrawal in cash";

            if (!_bankServices.CheckIfCustomerAccountBalanceIsValid(senderAccount.AccountId, viewModel.AmountToSend))
            {
                ModelState.AddModelError("AmountToSend", "You don't have enough money");
                return View(viewModel);
            }

            var senderTransaction = new Transactions
            {
                AccountId = viewModel.AccountId,
                Bank = viewModel.Bank,
                Account = viewModel.ToAccountId.ToString(),
                Balance = senderAccount.Balance - viewModel.AmountToSend,
                Amount = viewModel.AmountToSend * -1,
                Type = "Debit",
                Date = DateTime.Now,
                Operation = viewModel.Operation,
                Symbol = viewModel.MessageForSender,
                AccountNavigation = senderAccount
            };
            senderAccount.Balance -= viewModel.AmountToSend;

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

            _appDataContext.Add(senderTransaction);
            _appDataContext.SaveChanges();

            return RedirectToAction("TransactionConfirmed");
        }

        public IActionResult ValidateExistingAccountId(int AccountId)
        {
            return _bankServices.GetSpecificAccountFromDatabase(AccountId) == null ? Json($"No account with {AccountId} Id found") : Json(true);
        }
    }
}