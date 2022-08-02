using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Member User { get; set; }
    }
}
