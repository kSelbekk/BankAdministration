using System.ComponentModel.DataAnnotations;
using WebApplication7.Services;

namespace WebApplication7.Utilities
{
    public class ValidAccountBalanceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var viewModelBalance = (decimal)value;

            return ValidationResult.Success;
        }
    }
}