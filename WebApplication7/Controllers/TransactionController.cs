using Microsoft.AspNetCore.Mvc;

namespace WebApplication7.Controllers
{
    public class TransactionController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}