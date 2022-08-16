using Service.Interfaces;
using Service.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class OrderService : IOrder
    {
        private readonly NStoreContext _context;

        public OrderService(NStoreContext context)
        {
            _context = context;
        }

        public NotetiModel CreateOrder(CheckOutRequest input)
        {
            var result = new NotetiModel();

            var member = _context.Members.FirstOrDefault(x => x.Id.Equals(input.UserId));

            var discount = _context.Promotions.FirstOrDefault(x => x.Id.Equals(input.IdDisCount));

            var discountPercent = 0;

            if (discount != null)
            {
                discountPercent = discount.DiscountPercent;

                //add id discount unique to memberpromotion

                if (member.Promotions == null)
                {
                    member.Promotions = $",{discount.Id},";
                }
                else
                {
                    member.Promotions += $"{discount.Id},";
                }

                //minus discount amount
                discount.DiscountAmount--;
            }

            var order = new Order()
            {
                DiliveryPrice = 2,
                Discount = discountPercent,
                OrderDate = DateTime.Now,
                ShipAddress = input.Address,
                ShipEmail = input.Email,
                ShipName = input.Name,
                ShipPhoneNumber = input.PhoneNumber,
                Status = 1,
                UserId = member.Id,
                OrderDetails = input.OrderDetails.Select(x => new OrderDetail()
                {
                    Quantity = x.Quantity,
                    Price = _context.Products.FirstOrDefault(y => y.Id.Equals(x.ProductId)).Price,
                    ProductId = x.ProductId
                }).ToList()
            };
            _context.Orders.Add(order);

            var save = _context.SaveChanges();

            if (save > 0)
            {
                result.IsSuccess = true;
                result.Noteti = "Create order success!";
            }
            else
            {
                result.IsSuccess = false;
                result.Noteti = "Create order fail!";
            }
            return result;
        }
    }
}