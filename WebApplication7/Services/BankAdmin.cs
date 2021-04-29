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

        public IQueryable<Dispositions> GetSpecificDispositions(int id)
        {
            return _bankAppDataContext.Dispositions.Where(a => a.DispositionId == id);
        }

        public IQueryable<Customers> GetAllCustomersFromDatabase()
        {
            return _bankAppDataContext.Customers;
        }

        public IQueryable<Transactions> GetAllTransactionsFromSpecificCustomer(int id)
        {
            return _bankAppDataContext.Transactions.Where(c => c.AccountId == id).OrderByDescending(a => a);
        }
    }
}