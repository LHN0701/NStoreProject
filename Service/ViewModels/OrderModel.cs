using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
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
}