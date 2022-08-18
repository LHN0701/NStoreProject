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

        [HttpPost("GetAll")]
        public IActionResult GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iOrder.GetAll();

            return Ok(result);
        }

        [HttpPost("GetById")]
        public IActionResult GetById(OrderModel.Input.GetOrder request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iOrder.GetById(request);

            return Ok(result);
        }

        [HttpPost("DeleteOrder")]
        public IActionResult DeleteOrder(OrderModel.Input.DeteleOrder request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _iOrder.DeleteOrder(request);

            return Ok(result);
        }
    }
}