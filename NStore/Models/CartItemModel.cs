using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int IdDisCount { get; set; }
        public int DiscountPercent { get; set; }
    }

    public class CheckOutRequest
    {
        public int UserId { get; set; }
        public int IdDisCount { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your Address.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your PhoneNumber.")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        public List<OrderDetailModel> OrderDetails { set; get; } = new List<OrderDetailModel>();
    }

    public class OrderDetailModel
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }

    public class CostIncurred
    {
        public int IdDisCount { get; set; }
        public int DiscountPercent { get; set; }
    }
}