using Microsoft.AspNetCore.Mvc;
using WebApplication7.Data;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class BaseController : Controller
    {
        protected readonly BankAppDataContext _appDataContext;

        public BaseController(BankAppDataContext appDataContext)
        {
            _appDataContext = appDataContext;
        }
    }
}