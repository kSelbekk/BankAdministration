using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public class BankAdmin : IBankServices
    {
        private readonly BankAppDataContext _bankAppDataContext;

        public BankAdmin(BankAppDataContext bankAppDataContext)
        {
            _bankAppDataContext = bankAppDataContext;
        }

        public IQueryable<Dispositions> GetAllDispositionsFromDatabase()
        {
            return _bankAppDataContext.Dispositions
                .Include(c => c.Customer)
                .Include(a => a.Account);
        }

        public Customers GetSpecificCustomerInformation(int id)
        {
            return _bankAppDataContext.Customers
                .Include(a => a.Dispositions)
                .ThenInclude(a => a.Account)
                .FirstOrDefault(a => a.CustomerId == id);
        }

        public IQueryable<Customers> GetAllCustomersFromDatabase()
        {
            return _bankAppDataContext.Customers;
        }

        public IQueryable<Transactions> GetAllTransactionsFromSpecificCustomer(int id, int skip, int take)
        {
            return _bankAppDataContext.Transactions
                .Where(c => c.AccountId == id)
                .OrderByDescending(a => a.Date)
                .Skip(skip)
                .Take(take);
        }

        public Accounts GetSpecificAccountFromDatabase(int id)
        {
            return _bankAppDataContext.Accounts
                .FirstOrDefault(i => i.AccountId.Equals(id));
        }

        public bool CheckIfCustomerAccountBalanceIsValid(int accountId, decimal money)
        {
            var account = _bankAppDataContext.Accounts.First(i => i.AccountId == accountId);

            return account.Balance >= money;
        }
    }
}