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
        public int AccountId { get; set; }

        //Symbol in database
        [MaxLength(50)]
        public string MessageForSender { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Remote("ValidateNoNegativeNumber", "Transaction")]
        public decimal AmountToWithdrawal { get; set; }

        public string Operation { get; set; }
    }
}