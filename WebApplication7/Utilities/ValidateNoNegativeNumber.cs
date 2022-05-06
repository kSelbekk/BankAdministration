using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Utilities
{
    public class ValidateNoNegativeNumber : ValidationAttribute
    {
        public override bool IsValid(object? value) => (decimal)value > 0;
    }
}