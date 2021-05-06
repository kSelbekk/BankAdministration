using System;
using System.Linq;
using System.Net;
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
    //Credit                              Okänd insättning...
    //Credit in Cash                      insättning cash
    //Credit Card Withdrawal              uttag bankomat
    //Remittance to Another Bank          du överför pengar till en annan bank
    //Withdrawal in Cash                  uttag på banken liksom = cash

    //om du betalar till en annan i samma bank så kan du köra Withdrawal in cash på ditt konto.Och Credit in Cash på mottagaren

    //[Authorize(Roles = "Cashier")]
    public class TransactionController : BaseController
    {
        private readonly SignInManager<IdentityUser> _userManager;

        public TransactionController(BankAppDataContext appDataContext, IBankServices bankServices, SignInManager<IdentityUser> userManager)
            : base(appDataContext, bankServices)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DepositMoney()
        {
            var viewModel = new TransactionDepositMoenyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DepositMoney(TransactionDepositMoenyViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            var depositAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.ToAccountId);

            if (depositAccount == null)
            {
                ModelState.AddModelError("FromAccountId", "No account found");
                return View(viewModel);
            }

            viewModel.Operation = viewModel.Bank == null ? "Credit" : "Collection from Another Bank";

            var depositTransaction = new Transactions
            {
                AccountId = viewModel.ToAccountId,
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
            return View();
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

            var withdrawlAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.FromAccountId);

            if (withdrawlAccount == null)
            {
                ModelState.AddModelError("FromAccountId", "No account found");
                return View(viewModel);
            }

            if (withdrawlAccount.Balance < viewModel.AmountToWithdrawal)
            {
                ModelState.AddModelError("AmountToWithdrawal", "You don't have enough money");
                return View(viewModel);
            }

            viewModel.Operation = _userManager.IsSignedIn(User) ? "Credit Card Withdrawal" : "Withdrawal in Cash";

            var withdrawlTransaction = new Transactions
            {
                AccountId = viewModel.FromAccountId,
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

            return RedirectToAction("WithdrawalMoney");
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

            var senderAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.FromAccountId);

            var receiverAccount = _bankServices.GetSpecificAccountFromDatabase(viewModel.ToAccountId);

            viewModel.Operation = receiverAccount == null ? "Remittance to Another Bank" : "Withdrawal in cash";

            if (senderAccount == null)
            {
                ModelState.AddModelError("FromAccountId", "No account found");
                return View(viewModel);
            }

            if (senderAccount.Balance < viewModel.AmountToSend)
            {
                ModelState.AddModelError("AmountToSend", "You dont have ennough money");
                return View(viewModel);
            }

            var senderTransaction = new Transactions
            {
                AccountId = viewModel.FromAccountId,
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
                    Account = viewModel.FromAccountId.ToString(),
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

            return RedirectToAction("SendMoney");
        }
    }
}