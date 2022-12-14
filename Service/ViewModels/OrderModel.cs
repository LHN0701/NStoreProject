using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    //cart
    public class CheckOutRequest
    {
        public int UserId { get; set; }
        public int IdDisCount { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<OrderDetailModel> OrderDetails { set; get; } = new List<OrderDetailModel>();
    }

    public class OrderDetailModel
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }

    //order
    public class OrderModel
    {
        public class OrderDetailBase
        {
            public int OrderId { get; set; }

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public int Quantity { get; set; }

            public decimal Price { get; set; }
        }

        public class OrderBase
        {
            public int Id { get; set; }

            public DateTime OrderDate { get; set; }

            public int UserId { get; set; }

            public string UserName { get; set; }

            public string ShipName { get; set; }

            public string ShipAddress { get; set; }

            public string ShipEmail { get; set; }

            public string ShipPhoneNumber { get; set; }

            public int Status { get; set; }

            public decimal? DiliveryPrice { get; set; }

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