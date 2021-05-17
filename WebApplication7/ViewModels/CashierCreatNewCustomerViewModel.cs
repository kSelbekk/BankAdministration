using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.ViewModels
{
    public class CashierCreatNewCustomerViewModel
    {
        [Required, MaxLength(6)]
        public string Gender { get; set; }

        [Required, MaxLength(100)]
        public string Givenname { get; set; }

        [Required, MaxLength(100)]
        public string Surname { get; set; }

        [Required, MaxLength(100)]
        public string Streetaddress { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(15)]
        public string Zipcode { get; set; }

        [Required, MaxLength(100)]
        public string Country { get; set; }

        [Required]
        public string CountryCode { get; set; }

        public DateTime? Birthday { get; set; }

        [MaxLength(20)]
        public string? NationalId { get; set; }

        [MaxLength(10)]
        public string? Telephonecountrycode { get; set; }

        [MaxLength(25)]
        public string? Telephonenumber { get; set; }

        [MaxLength(100), EmailAddress]
        public string? Emailaddress { get; set; }

        public ICollection<CashierCreatNewDispositionsViewModel> ListDespositions { get; set; }
    }
}