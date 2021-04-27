using System;
using System.Collections.Generic;

namespace WebApplication7.ViewModels
{
    public class StartPageModel
    {
        public class ListCustomersViewModel
        {
            public int CustomerId { get; set; }
            public string FullName { get; set; }
            public DateTime? PersonalNumber { get; set; } = new DateTime();
            public string Address { get; set; }
            public string City { get; set; }
        }

        public class HomeIndexViewModel
        {
            public int TotalCustomers { get; set; }
            public int TotalAccounts { get; set; }
            public decimal TotalBalance { get; set; }
        }

        public List<ListCustomersViewModel> ListCustomersViewModels { get; set; }
        public HomeIndexViewModel HomeIndexViewModels { get; set; }
    }
}