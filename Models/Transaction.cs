using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankingCoreApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }
    }
}