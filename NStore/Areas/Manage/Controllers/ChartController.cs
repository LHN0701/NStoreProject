using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class ChartController : Controller
    {
        [HttpGet]
        public JsonResult ColumnChart()
        {
            var data = Utilities.SendDataRequest<List<ChartModel.OneMonTotalSale>>(ConstantValues.Chart.SaleSixMon);

            return Json(data);
        }

        [HttpGet]
        public JsonResult AreaChart()
        {
            var data = Utilities.SendDataRequest<List<ChartModel.ExpenseAndSale>>(ConstantValues.Chart.ExpenseAndSale);

            return Json(data);
        }
    }
}