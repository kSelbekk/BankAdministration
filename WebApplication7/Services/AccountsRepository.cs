using System;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public class AccountsRepository
    {
        private readonly BankAppDataContext _bankAppDataContext;

        public AccountsRepository(BankAppDataContext bankAppDataContext)
        {
            _bankAppDataContext = bankAppDataContext;
        }

        public void CreateNewAccount(Customers newCustomer)
        {
            var newAccount = new Accounts
            {
                Created = DateTime.Now,
                Frequency = "Monthly"
            };

            _bankAppDataContext.Accounts.Update(newAccount);

            var newDisposition = new Dispositions
            {
                Account = newAccount,
                Customer = newCustomer,
                Type = "OWNER"
            };

            _bankAppDataContext.Dispositions.Update(newDisposition);
        }
        public void SaveChanges()
        {
            _bankAppDataContext.SaveChanges();
        }
    }
}
