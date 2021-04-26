using Microsoft.AspNetCore.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(BankAppDataContext appDataContext) : base(appDataContext)
        {
        }

        // GET
        public IActionResult CustomerProfile()
        {
            return View();
        }
    }
}