using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;
using WebApplication7.Services;

namespace WebApplication7.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(BankAppDataContext appDataContext, IBankServices bankServices) : base(appDataContext, bankServices)
        {
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}