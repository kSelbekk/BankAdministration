using System;
using WebApplication7.Models;

namespace WebApplication7.ViewModels
{
    public class CustomerCustomerProfileViewModel
    {
        public int Id { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public DateTime? Birthday { get; set; } = new DateTime();
        public string? NationalId { get; set; }
        public string? TelephoneCountryCode { get; set; }
        public string? Telephonenumber { get; set; }
        public string? Emailaddress { get; set; }
        public Accounts Account { get; set; }
        public decimal TotalBalance { get; set; }
    }
}