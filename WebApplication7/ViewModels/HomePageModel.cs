using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication7.ViewModels
{
    public class HomePageModel
    {
        public int Id { get; set; }

        public class HomeIndexViewModel
        {
            public int TotalCustomers { get; set; }
            public int TotalAccounts { get; set; }
            public decimal TotalBalance { get; set; }
        }

        public class InformationPerCountry
        {
            public List<NumberOfCustomersPerCountry> CustomersPerCountries { get; set; }

            public class NumberOfCustomersPerCountry
            {
                public string Country { get; set; }
                public int Accounts { get; set; }
                public decimal TotalBalance { get; set; }
            }
        }

        public InformationPerCountry CountryInformation { get; set; }
        public HomeIndexViewModel IndexInformation { get; set; }
    }
}