using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication7.ViewModels
{
    public class TransactionDepositMoneyViewModel
    {
        [Required]
        [Range(1, Double.MaxValue, ErrorMessage = "Input a valid account-id")]
        public int ToAccountId { get; set; }

        //Symbol in database
        [MaxLength(50)]
        public string MessageForReceiver { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, Double.MaxValue, ErrorMessage = "Input a valid amount")]
        public decimal AmountToDeposit { get; set; }

        [MaxLength(2)]
        public string Bank { get; set; }

        public string Operation { get; set; }
    }
}