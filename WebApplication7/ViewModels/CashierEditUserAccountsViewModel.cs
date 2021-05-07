using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication7.ViewModels
{
    public class CashierEditUserAccountsViewModel
    {
        public int Id { get; set; }
        [MaxLength(256), Required]
        public string Givenname { get; set; }
        [MaxLength(256), Required]
        public string Surname { get; set; }
        [MaxLength(256), Required]
        public string StreetAddress { get; set; }
        [Required]
        public string Gender { get; set; }
        [MaxLength(256), Required]
        public string City { get; set; }
        [MaxLength(50), Required]
        public string Zipcode { get; set; }
        [MaxLength(256), Required]
        public string Country { get; set; }
        [MaxLength(256), Required]
        public string CountryCode { get; set; }
        [MaxLength(50)]
        public string? NationalId { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? TelephoneCountryCode { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Telephonenumber { get; set; }
        [EmailAddress]
        public string? EmailAddress { get; set; }
    }
}