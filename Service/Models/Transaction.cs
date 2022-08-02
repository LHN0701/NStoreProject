using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Amount { get; set; }
        public decimal Fee { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public string Provider { get; set; }
        public int UserId { get; set; }

        public virtual Member User { get; set; }
    }
}
