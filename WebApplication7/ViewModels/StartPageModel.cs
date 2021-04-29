using System;
using System.Collections.Generic;

namespace WebApplication7.ViewModels
{
    public class StartPageModel
    {
        public class HomeIndexViewModel
        {
            public int TotalCustomers { get; set; }
            public int TotalAccounts { get; set; }
            public decimal TotalBalance { get; set; }
        }

        public HomeIndexViewModel HomeIndexViewModels { get; set; }
    }
}