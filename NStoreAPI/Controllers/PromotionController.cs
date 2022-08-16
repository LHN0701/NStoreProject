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
    public class PromotionController : ControllerBase
    {
        private readonly IPromotion _iPromotion;

        public PromotionController(IPromotion iPromotion)
        {
            _iPromotion = iPromotion;
        }

        [HttpPost("GetAll")]
        public IActionResult GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iPromotion.GetAll();

            return Ok(result);
        }

        [HttpPost("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iPromotion.GetById(id);

            return Ok(result);
        }

        [HttpPost("GetPromotionClient")]
        public IActionResult GetPromotionClient(PromotionModel.Input.GetPromotionClient request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iPromotion.GetPromotionClient(request);

            return Ok(result);
        }

        [HttpPost("CreatePromotion")]
        public IActionResult CreatePromotion(PromotionModel.PromotionBase request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iPromotion.CreatePromotion(request);

            return Ok(result);
        }

        [HttpPost("UpdatePromotion")]
        public IActionResult UpdatePromotion(PromotionModel.PromotionBase request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iPromotion.UpdatePromotion(request);

            return Ok(result);
        }

        [HttpPost("DeletePromotion")]
        public IActionResult DeletePromotion(PromotionModel.Input.DeletePromotion request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _iPromotion.DeletePromotion(request);

            return Ok(result);
        }
    }
}