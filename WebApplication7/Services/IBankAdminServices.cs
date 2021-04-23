using System.Collections.Generic;
using System.Linq;
using WebApplication7.Models;

namespace WebApplication7.Services
{
    public interface IBankAdminServices
    {
        public IQueryable<Dispositions> GetAllDispositionsFromDatabase();
    }

    public class BankAdmin : IBankAdminServices
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
    }
}