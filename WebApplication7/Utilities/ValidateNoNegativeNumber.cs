using System.ComponentModel.DataAnnotations;
using WebApplication7.Services;

namespace WebApplication7.Utilities
{
    public class ValidateNoNegativeNumber : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if ((decimal)value > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("No negative numbers");
        }
    }
}