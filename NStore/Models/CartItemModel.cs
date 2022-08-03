using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Models
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CheckOutModel
    {
        public List<CartItemModel> CartItems { get; set; }

        public CheckOutRequest CheckoutModel { get; set; }
    }

    public class CheckOutRequest
    {
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
}