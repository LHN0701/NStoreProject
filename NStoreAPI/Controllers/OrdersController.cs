using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrder _iOrder;

        public OrdersController(IOrder iOrder)
        {
            _iOrder = iOrder;
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(CheckOutRequest input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iOrder.CreateOrder(input);

            return Ok(result);
        }
    }
}