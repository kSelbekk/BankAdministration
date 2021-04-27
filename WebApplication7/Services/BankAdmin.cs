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
            return _bankAppDataContext.Dispositions;
        }

        public Customers GetSpecificCustomerFromDatabase(int id)
        {
            return _bankAppDataContext.Customers.FirstOrDefault(i => i.CustomerId == id);
        }

        public IQueryable<Accounts> GetBankAccountsFromCustomer(int id)
        {
            return _bankAppDataContext.Accounts.Where(a => a.AccountId == id);
        }

        public IQueryable<Customers> GetAllCustomersFromDatabase()
        {
            return _bankAppDataContext.Customers;
        }
    }
}