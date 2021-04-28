using System.Collections.Generic;
using System.Linq;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public interface IBankServices
    {
        public IQueryable<Dispositions> GetAllDispositionsFromDatabase();

        public Customers GetSpecificCustomerFromDatabase(int id);

        public IQueryable<Dispositions> GetBankAccountsFromCustomer(int id);

        public IQueryable<Customers> GetAllCustomersFromDatabase();
    }
}