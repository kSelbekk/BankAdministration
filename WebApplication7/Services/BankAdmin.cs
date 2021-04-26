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

        public Dispositions GetSpecificCustomerFromDatabase(int id)
        {
            return _bankAppDataContext.Dispositions
                .Include(a => a.Account)
                .Include(a => a.Customer)
                .FirstOrDefault(i => i.CustomerId == id);
        }
    }
}