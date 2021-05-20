using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication7.Controllers;
using WebApplication7.Services;
using System.Runtime.InteropServices.ComTypes;

using AutoFixture;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using WebApplication7.ViewModels;

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
            _bankMockServices = new Mock<IBankServices>();

            var options = new DbContextOptionsBuilder<BankAppDataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            ctx = new BankAppDataContext(options);

            _sut = new TransactionController(ctx, _bankMockServices.Object);
        }

        [TestMethod]
        public void Cant_complete_transaction_if_balance_is_less_then_amount_to_send()
        {
            var viewModel = fixture.Create<TransactionSendMoneyViewModel>();

            viewModel.AmountToSend = 1337;

            _bankMockServices.Setup(e =>
                e.CheckIfCustomerAccountBalanceIsValid(viewModel.AccountId, viewModel.AmountToSend)).Returns(true);
        }
    }
}