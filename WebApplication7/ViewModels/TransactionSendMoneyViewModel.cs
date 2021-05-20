using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WebApplication7.Utilities;

namespace WebApplication7.ViewModels
{
    public class TransactionSendMoneyViewModel
    {
        [Remote("ValidateExistingAccountId", "Transaction")]
        [Required]
        public int AccountId { get; set; }

        public int ToAccountId { get; set; }

        [Required]
        public decimal AmountToSend { get; set; }

        [MaxLength(50)]
        public string? MessageForSender { get; set; }

        [MaxLength(50)]
        public string? MessageForReceiver { get; set; }

        public string Operation { get; set; }

        public string Type { get; set; }

        [StringLength(2)]
        public string? Bank { get; set; }
    }
}