using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication7.ViewModels
{
    public class TransactionDepositMoneyViewModel
    {
        [Required]
        [Remote("ValidateExistingAccountId", "Transaction")]
        public int AccountId { get; set; }

        //Symbol in database
        [MaxLength(50)]
        public string MessageForReceiver { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Remote("ValidateNoNegativeNumber", "Transaction")]
        public decimal AmountToSend { get; set; }

        [MaxLength(2)]
        public string Bank { get; set; }
    }
}