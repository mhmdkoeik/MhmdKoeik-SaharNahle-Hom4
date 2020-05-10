using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MhmdKoeik_HomeWork3.Models
{
    public class Transfer
    {
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public int CheckingAccountId { get; set; }

        [Required]
        [Display(Name = "To Account #")]
        public string TransactionSource { get; set; }

        [DataType(DataType.DateTime)]
        public string TransactionDate{get; set;}
    }
}