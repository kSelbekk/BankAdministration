using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;
using WebApplication7.Services;

namespace WebApplication7.Controllers
{
    public class BaseController : Controller
    {
        protected readonly BankAppDataContext _appDataContext;
        protected readonly IBankServices _bankServices;

        public BaseController(BankAppDataContext appDataContext, IBankServices bankServices)
        {
            _appDataContext = appDataContext;
            _bankServices = bankServices;
        }
    }
}