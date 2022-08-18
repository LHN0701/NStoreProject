using Microsoft.AspNetCore.Mvc;
using NStore.Areas.Manage.Models.Authorization;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize("Admin", "Staff")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            if (TempData["result"] != null)
            {
                ViewBag.Successmsg = TempData["result"];
            }
            var listOrder = Utilities.SendDataRequest<List<OrderModel.Output.GetOrder>>(ConstantValues.Order.GetAll);

            return View(listOrder);
        }

        [HttpGet]
        public IActionResult DeleteOrder(OrderModel.Input.DeteleOrder request)
        {
            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Order.DeleteOrder, request);

            if (result.Issuccess)
            {
                TempData["result"] = result.Noteti;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Noteti);
            return RedirectToAction("Index");
        }
    }
}