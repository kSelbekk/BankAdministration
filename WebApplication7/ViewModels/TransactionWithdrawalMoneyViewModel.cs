using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication7.ViewModels
{
    public class TransactionWithdrawalMoneyViewModel
    {
        [Required]
        [Remote("ValidateExistingAccountId", "Transaction")]
        [Range(1, double.MaxValue, ErrorMessage = "Input a valid account-id")]
        public int FromAccountId { get; set; }

        //Symbol in database
        [MaxLength(50)]
        public string MessageForSender { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, Double.MaxValue, ErrorMessage = "Input a valid amount")]
        public decimal AmountToWithdrawal { get; set; }

        public string Operation { get; set; }
    }
}