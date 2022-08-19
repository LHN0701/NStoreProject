using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataChartController : ControllerBase
    {
        private readonly IChart _iChart;

        public DataChartController(IChart iChart)
        {
            _iChart = iChart;
        }

        [HttpPost("SaleSixMon")]
        public IActionResult SaleSixMon()
        {
            var result = _iChart.SixMonTotalSale();
            return Ok(result);
        }

        [HttpPost("ExpenseAndSale")]
        public IActionResult ExpenseAndSale()
        {
            var result = _iChart.ExpenseAndSale();
            return Ok(result);
        }
    }
}