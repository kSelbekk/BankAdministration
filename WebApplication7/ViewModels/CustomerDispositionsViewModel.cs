using System.Collections.Generic;
using WebApplication7.Models;

namespace WebApplication7.ViewModels
{
    public class CustomerDispositionsViewModel
    {
        public int DispositionId { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public string Type { get; set; }

        public CustomerAccountViewModel Account { get; set; }
    }
}