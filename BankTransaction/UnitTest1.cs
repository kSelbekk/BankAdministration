using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication7.Controllers;
using WebApplication7.Services;
using System.Runtime.InteropServices.ComTypes;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using WebApplication7.Models;
using WebApplication7.ViewModels;
using WebApplication7.Data;

namespace BankTransaction
{
    public class BaseTest
    {
        protected AutoFixture.Fixture fixture = new AutoFixture.Fixture();
    }

    [TestClass]
    public class TransactionUnitTest : BaseTest
    {
        private Mock<IBankServices> _bankMockServices;
        private TransactionController _sut;
        private BankAppDataContext ctx;

        public TransactionUnitTest()
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var options = new DbContextOptionsBuilder<BankAppDataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            ctx = new BankAppDataContext(options);

            ctx.Database.EnsureCreated();

            var acc = fixture.Create<Accounts>();
            acc.AccountId = 1;
            acc.Balance = 1000;

            var account1 = fixture.Create<Accounts>();
            account1.AccountId = 2;
            account1.Balance = 100;

            ctx.Add(acc);
            ctx.Add(account1);
            ctx.SaveChanges();

            _bankMockServices = new Mock<IBankServices>();

            _sut = new TransactionController(ctx, _bankMockServices.Object);
        }

        [TestMethod]
        public void Dont_complete_transactionSendMoney_if_balance_is_less_then_amount_to_send()
        {
            var viewModel = fixture.Create<TransactionSendMoneyViewModel>();
            viewModel.AccountId = 1;
            viewModel.ToAccountId = 2;
            viewModel.AmountToSend = 50;

            _bankMockServices.Setup(e =>
                e.CheckIfCustomerAccountBalanceIsValid(viewModel.AccountId, viewModel.AmountToSend)).Returns(true);
            _sut.SendMoney(viewModel);
        }

        [TestMethod]
        public void Cant_complete_transactionWithdrawl_if_balance_is_less_then_amount_to_send()
        {
            var viewModel = fixture.Create<TransactionWithdrawalMoneyViewModel>();
            viewModel.AmountToSend = 69;

            _bankMockServices.Setup(e =>
                e.CheckIfCustomerAccountBalanceIsValid(viewModel.AccountId, viewModel.AmountToSend)).Returns(true);
        }

        [TestMethod]
        public void Not_supposed_to_use_negative_numbers_for_transactions()
        {
        }
    }
}