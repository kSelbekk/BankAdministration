using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication7.ViewModels
{
    public class HomePageModel
    {
        public class HomeIndexViewModel
        {
            public int TotalCustomers { get; set; }
            public int TotalAccounts { get; set; }
            public decimal TotalBalance { get; set; }
        }

        public class InformationPerCountry
        {
            public List<NumberOfCustomersPerCountry> TotCustomersPerCountries { get; set; }
            public List<SumOfTotalBalancePerCountry> BalancePerCountries { get; set; }
            public List<NumberOfAccountsPerCountry> AccountsPerCountries { get; set; }

            public class NumberOfCustomersPerCountry
            {
                public string Country { get; set; }
                public int NumberOfCustomers { get; set; }
            }

            public class SumOfTotalBalancePerCountry
            {
                public string Country { get; set; }
                public decimal TotalBalance { get; set; }
            }

            public class NumberOfAccountsPerCountry
            {
                public string Country { get; set; }
                public int NumberOfAccounts { get; set; }
            }
        }

        public List<InformationPerCountry> CountryInformation { get; set; }
        public HomeIndexViewModel IndexInformation { get; set; }
    }
}