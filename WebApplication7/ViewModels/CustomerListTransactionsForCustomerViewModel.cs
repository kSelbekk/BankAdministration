using System;
using System.Collections.Generic;

namespace WebApplication7.ViewModels
{
    public class CustomerListTransactionsForCustomerViewModel
    {
        public List<CustomerTransaction> CustomerTransactions { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public class CustomerTransaction
        {
            public int TransactionId { get; set; }
            public DateTime TransactionDate { get; set; } = new DateTime();
            public string Type { get; set; }
            public string Operation { get; set; }
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }
            public string Symbol { get; set; }
            public string Bank { get; set; }
            public string Account { get; set; }
        }
    }
}