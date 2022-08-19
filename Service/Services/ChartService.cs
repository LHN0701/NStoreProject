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
    public class ChartService : IChart
    {
        private readonly NStoreContext _context;

        public ChartService(NStoreContext context)
        {
            _context = context;
        }

        public List<ChartModel.OneMonTotalSale> SixMonTotalSale()
        {
            var query = from o in _context.Orders
                        join od in _context.OrderDetails on o.Id equals od.OrderId into ood
                        from od in ood.DefaultIfEmpty()
                        where od.OrderId == o.Id
                        select new { o, od };

            var result = new List<ChartModel.OneMonTotalSale>();

            var now = DateTime.Now;

            var monOne = new ChartModel.OneMonTotalSale();
            monOne.Month = now.Month;

            var monTwo = new ChartModel.OneMonTotalSale();
            monTwo.Month = now.AddMonths(-1).Month;

            var monThr = new ChartModel.OneMonTotalSale();
            monThr.Month = now.AddMonths(-2).Month;

            var monFor = new ChartModel.OneMonTotalSale();
            monFor.Month = now.AddMonths(-3).Month;

            var monFiv = new ChartModel.OneMonTotalSale();
            monFiv.Month = now.AddMonths(-4).Month;

            var monSix = new ChartModel.OneMonTotalSale();
            monSix.Month = now.AddMonths(-5).Month;

            foreach (var order in query)
            {
                if (now.Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monOne.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-1).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monTwo.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-2).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monThr.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-3).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monFor.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-4).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monFiv.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-5).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monSix.TotalSaleOfMon += total ?? 0;
                }
            }
            result.Add(monOne);
            result.Add(monTwo);
            result.Add(monThr);
            result.Add(monFor);
            result.Add(monFiv);
            result.Add(monSix);

            return result;
        }

        public List<ChartModel.ExpenseAndSale> ExpenseAndSale()
        {
            var query = from o in _context.Orders
                        join od in _context.OrderDetails on o.Id equals od.OrderId into ood
                        from od in ood.DefaultIfEmpty()
                        where od.OrderId == o.Id
                        select new { o, od };

            var result = new List<ChartModel.ExpenseAndSale>();

            var now = DateTime.Now;

            var monOne = new ChartModel.ExpenseAndSale();
            monOne.Month = now.Month;

            var monTwo = new ChartModel.ExpenseAndSale();
            monTwo.Month = now.AddMonths(-1).Month;

            var monThr = new ChartModel.ExpenseAndSale();
            monThr.Month = now.AddMonths(-2).Month;

            var monFor = new ChartModel.ExpenseAndSale();
            monFor.Month = now.AddMonths(-3).Month;

            var monFiv = new ChartModel.ExpenseAndSale();
            monFiv.Month = now.AddMonths(-4).Month;

            var monSix = new ChartModel.ExpenseAndSale();
            monSix.Month = now.AddMonths(-5).Month;

            foreach (var order in query)
            {
                if (now.Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monOne.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-1).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monTwo.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-2).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monThr.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-3).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monFor.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-4).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monFiv.TotalSaleOfMon += total ?? 0;
                }
                else if (now.AddMonths(-5).Month == order.o.OrderDate.Month)
                {
                    var total = order.od.Price * order.od.Quantity - order.o.DiliveryPrice;
                    if (order.o.Discount != null)
                        total = total - (total * order.o.Discount / 100);
                    monSix.TotalSaleOfMon += total ?? 0;
                }
            }

            var products = _context.Products.ToList();

            foreach (var product in products)
            {
                if (now.Month == product.DateCreated.Month)
                {
                    monOne.TotalExpense += product.OriginalPrice;
                }
                else if (now.AddMonths(-1).Month == product.DateCreated.Month)
                {
                    monTwo.TotalExpense += product.OriginalPrice;
                }
                else if (now.AddMonths(-2).Month == product.DateCreated.Month)
                {
                    monThr.TotalExpense += product.OriginalPrice;
                }
                else if (now.AddMonths(-3).Month == product.DateCreated.Month)
                {
                    monFor.TotalExpense += product.OriginalPrice;
                }
                else if (now.AddMonths(-4).Month == product.DateCreated.Month)
                {
                    monFiv.TotalExpense += product.OriginalPrice;
                }
                else if (now.AddMonths(-5).Month == product.DateCreated.Month)
                {
                    monSix.TotalExpense += product.OriginalPrice;
                }
            }

            result.Add(monOne);
            result.Add(monTwo);
            result.Add(monThr);
            result.Add(monFor);
            result.Add(monFiv);
            result.Add(monSix);

            return result;
        }
    }
}