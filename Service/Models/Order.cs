using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }
        public int Status { get; set; }
        public decimal? DiliveryPrice { get; set; }
        public int? Discount { get; set; }

        public virtual Member User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
