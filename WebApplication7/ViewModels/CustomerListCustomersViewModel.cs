using System;
using System.Collections.Generic;

namespace WebApplication7.ViewModels
{
    public class CustomerListCustomersViewModel
    {
        public class ListCustomerViewModel
        {
            public int CustomerId { get; set; }
            public string FullName { get; set; }
            public DateTime? PersonalNumber { get; set; } = new DateTime();
            public string Address { get; set; }
            public string City { get; set; }
        }

        public List<ListCustomerViewModel> CustomersViewModels { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public string q { get; set; }
    }
}