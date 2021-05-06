using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication7.ViewModels
{
    public class TransactionSendMoneyViewModel
    {
        [Range(1, double.MaxValue, ErrorMessage = "Input a valid account-id")]
        [Required]
        public int FromAccountId { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Input a valid account-id")]
        public int ToAccountId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, double.MaxValue, ErrorMessage = "Input a valid amount")]
        public decimal AmountToSend { get; set; }

        //Symbol in database
        [MaxLength(50)]
        public string? MessageForSender { get; set; }

        //Symbol in database
        [MaxLength(50)]
        public string? MessageForReceiver { get; set; }

        [Required]
        public string Operation { get; set; }

        public string Type { get; set; }

        [StringLength(2)]
        public string? Bank { get; set; }
    }
}