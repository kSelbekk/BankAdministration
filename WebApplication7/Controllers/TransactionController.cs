using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;
using WebApplication7.Services;
using WebApplication7.ViewModels;

namespace WebApplication7.Controllers
{
    //Credit                              Okänd insättning...
    //Collection from Another Bank        nån på annan bank har betalat in på ditt konto
    //Credit in Cash                      insättning cash
    //Credit Card Withdrawal              uttag bankomat
    //Remittance to Another Bank          du överför pengar till en annan bank
    //Withdrawal in Cash                  uttag på banken liksom = cash

    //om du betalar till en annan i samma bank så kan du köra Withdrawal in cash på ditt konto.Och Credit in Cash på mottagaren

    public class TransactionController : BaseController
    {
        public TransactionController(BankAppDataContext appDataContext, IBankServices bankServices)
            : base(appDataContext, bankServices)
        {
        }

        public IActionResult WithdrawalMoney()
        {
            var viewModel = new TransactionWithdrawalMoneyViewModel();
            return View(viewModel);
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

            senderAccount.Balance -= viewModel.AmountToSend;

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

            if (receiverAccount != null)
            {
                viewModel.Operation = "Credit in Cash";
                receiverAccount.Balance += viewModel.AmountToSend;

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
                _appDataContext.Add(receiverTransaction);
            }

            _appDataContext.Add(senderTransaction);
            _appDataContext.SaveChanges();

            return RedirectToAction("SendMoney");
        }
    }
}