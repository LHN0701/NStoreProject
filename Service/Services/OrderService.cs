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

        public List<OrderModel.Output.GetOrder> GetAll()
        {
            var orders = _context.Orders.ToList();

            var result = new List<OrderModel.Output.GetOrder>();

            foreach (var item in orders)
            {
                var orderDetails = _context.OrderDetails.Where(x => x.OrderId.Equals(item.Id));

                var order = new OrderModel.Output.GetOrder()
                {
                    Order = new OrderModel.OrderBase()
                    {
                        DiliveryPrice = item.DiliveryPrice,
                        Discount = item.Discount,
                        Id = item.Id,
                        OrderDate = item.OrderDate,
                        ShipAddress = item.ShipAddress,
                        ShipEmail = item.ShipEmail,
                        ShipName = item.ShipName,
                        ShipPhoneNumber = item.ShipPhoneNumber,
                        Status = item.Status,
                        UserId = item.UserId,
                        UserName = _context.Members.FirstOrDefault(a => a.Id.Equals(item.UserId)).Name
                    },
                    OrderDetails = orderDetails.Select(x => new OrderModel.OrderDetailBase()
                    {
                        OrderId = x.OrderId,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        ProductName = _context.Products.FirstOrDefault(a => a.Id.Equals(x.ProductId)).Name,
                        Quantity = x.Quantity
                    }).ToList()
                };
                result.Add(order);
            }

            return result;
        }

        public List<OrderModel.Output.GetOrder> GetById(OrderModel.Input.GetOrder request)
        {
            var orders = _context.Orders.Where(x => x.UserId.Equals(request.MemberId)).ToList();

            var result = new List<OrderModel.Output.GetOrder>();

            foreach (var item in orders)
            {
                if (item.Id == request.OrderId)
                {
                    item.Status = request.Status ?? 1;
                    _context.Orders.FirstOrDefault(x => x.Id.Equals(request.OrderId)).Status = request.Status ?? 1;
                    _context.SaveChanges();
                }

                var orderDetails = _context.OrderDetails.Where(x => x.OrderId.Equals(item.Id));

                var order = new OrderModel.Output.GetOrder()
                {
                    Order = new OrderModel.OrderBase()
                    {
                        DiliveryPrice = item.DiliveryPrice,
                        Discount = item.Discount,
                        Id = item.Id,
                        OrderDate = item.OrderDate,
                        ShipAddress = item.ShipAddress,
                        ShipEmail = item.ShipEmail,
                        ShipName = item.ShipName,
                        ShipPhoneNumber = item.ShipPhoneNumber,
                        Status = item.Status,
                        UserId = item.UserId,
                        UserName = _context.Members.FirstOrDefault(a => a.Id.Equals(item.UserId)).Name
                    },
                    OrderDetails = orderDetails.Select(x => new OrderModel.OrderDetailBase()
                    {
                        OrderId = x.OrderId,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        ProductName = _context.Products.FirstOrDefault(a => a.Id.Equals(x.ProductId)).Name,
                        Quantity = x.Quantity
                    }).ToList()
                };
                result.Add(order);
            }

            return result;
        }

        public NotetiModel DeleteOrder(OrderModel.Input.DeteleOrder request)
        {
            var result = new NotetiModel();
            var order = _context.Orders.FirstOrDefault(x => x.Id.Equals(request.Id));
            var orderDetails = _context.OrderDetails.Where(x => x.OrderId.Equals(request.Id));
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.OrderDetails.RemoveRange(orderDetails);
            }
            var change = _context.SaveChanges();
            if (change > 0)
            {
                result.IsSuccess = true;
                result.Noteti = "Delete order success!";
            }
            else
            {
                result.IsSuccess = false;
                result.Noteti = "Delete order fail!";
            }

            return result;
        }
    }
}