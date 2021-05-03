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

            var senderTransaction = new Transactions();

            var senderAccountBalance =
                _bankServices.GetSpecificAccountFromDatabase(viewModel.FromAccountId).Balance;

            if (senderAccountBalance < viewModel.AmountToSend)
            {
                ModelState.AddModelError("AmountToSend", "You dont have ennough money");
                return View(viewModel);
            }

            var newAccountBalance = _bankServices.GetSpecificAccountFromDatabase(viewModel.FromAccountId).Balance -
                                    viewModel.AmountToSend;

            senderTransaction.AccountId = viewModel.FromAccountId;
            senderTransaction.Bank = viewModel.Bank;
            senderTransaction.Account = viewModel.ToAccountId.ToString();
            senderTransaction.Balance = newAccountBalance;
            senderTransaction.Amount = viewModel.AmountToSend;
            senderTransaction.Type = "Debit";
            senderTransaction.Date = viewModel.TransactionDate;

            //sender type = depit receiver = credit

            if (viewModel.ToAccountId != null)
            {
                var receiverTranasactions = new Transactions();
            }

            return RedirectToAction("SendMoney");
        }
    }
}