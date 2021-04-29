using System.Collections.Generic;
using System.Linq;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public interface IBankServices
    {
        public IQueryable<Dispositions> GetAllDispositionsFromDatabase();

        public IQueryable<Dispositions> GetSpecificDispositions(int id);

        public IQueryable<Customers> GetAllCustomersFromDatabase();

        public IQueryable<Transactions> GetAllTransactionsFromSpecificCustomer(int id);
    }
}