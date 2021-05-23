using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.Utilities;

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
        [ValidateNoNegativeNumber()]
        public decimal AmountToSend { get; set; }

        public string Operation { get; set; }
    }
}