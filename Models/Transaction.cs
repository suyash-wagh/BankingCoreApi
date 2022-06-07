using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankingCoreApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }

        [Range(500,1000000, ErrorMessage = "Minimum amount of transaction should be 500 and 1000000 for maximum.")]
        public double Amount { get; set; }
        public string TransactionType { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}