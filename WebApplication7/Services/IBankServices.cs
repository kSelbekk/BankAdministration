using System.Collections.Generic;
using System.Linq;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public interface IBankServices
    {
        public IQueryable<Dispositions> GetAllDispositionsFromDatabase();

        public IQueryable<Customers> GetAllCustomersFromDatabase();

        public IQueryable<Transactions> GetAllTransactionsFromSpecificCustomer(int id, int skip, int take);

        public Accounts GetSpecificAccountFromDatabase(int id);

        public Customers GetSpecificCustomerInformation(int id);

        public bool CheckIfCustomerAccountBalanceIsValid(int customerId, decimal money);

        public void WithdralTransaction(int accountId, string toAccountId, decimal amount, string message, string operation, string bank);

        public void DepositTransaction(int accountId, string fromAccountId, decimal amount, string operation, string bank, string messageForReceiver);
    }
}