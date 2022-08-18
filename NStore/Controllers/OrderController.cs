using Microsoft.AspNetCore.Mvc;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult GetOrder(OrderModel.Input.GetOrder request)
        {
            var listOrder = Utilities.SendDataRequest<List<OrderModel.Output.GetOrder>>(ConstantValues.Order.GetById, request);

            return View(listOrder);
        }
    }
}