using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingCoreApi.DTOs
{
    public class TransactionDTO
    {
        [Required(ErrorMessage = "Please enter Amount")]
        [Range(500, 1000000, ErrorMessage = "The field {0} must be greater than {1}.")]
        public double Amount { get; set; }
        public string TransactionType { get; set; }
    }
}