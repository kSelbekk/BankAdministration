using System.ComponentModel.DataAnnotations;
using WebApplication7.Services;

namespace WebApplication7.Utilities
{
    public class ValidateNoNegativeNumber : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if ((decimal)value > 0)
            {
                return true;
            }

            return false;
        }
    }
}