using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class OrderModel
    {
        public class OrderDetailBase
        {
            public int OrderId { get; set; }

            public int ProductId { get; set; }

            [Display(Name = "ProductName")]
            public string ProductName { get; set; }

            [Display(Name = "Quantity")]
            public int Quantity { get; set; }

            [Display(Name = "Price")]
            public decimal Price { get; set; }
        }

        public class OrderBase
        {
            [Display(Name = "Code order")]
            public int Id { get; set; }

            [Display(Name = "Order Date")]
            public DateTime OrderDate { get; set; }

            [Display(Name = "UserId")]
            public int UserId { get; set; }

            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Display(Name = "Name Shipper")]
            public string ShipName { get; set; }

            [Display(Name = "Customer's Address")]
            public string ShipAddress { get; set; }

            [Display(Name = "Customer's Email")]
            public string ShipEmail { get; set; }

            [Display(Name = "Customer's PhoneNuber")]
            public string ShipPhoneNumber { get; set; }

            [Display(Name = "Status")]
            public int Status { get; set; }

            [Display(Name = "DiliveryPrice")]
            public decimal? DiliveryPrice { get; set; }

            [Display(Name = "Discount")]
            public int? Discount { get; set; }
        }

        public class Input
        {
            public class DeteleOrder
            {
                public int Id { get; set; }
            }

            public class GetOrder
            {
                public int MemberId { get; set; }
                public int? OrderId { get; set; }

                public int? Status { get; set; }
            }
        }

        public class Output
        {
            public class GetOrder
            {
                public OrderBase Order { get; set; }

                public List<OrderDetailBase> OrderDetails { get; set; }
            }
        }
    }
}