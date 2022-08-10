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
    public class SlideController : ControllerBase
    {
        private readonly ISlide _iSlide;

        public SlideController(ISlide iSlide)
        {
            _iSlide = iSlide;
        }

        [HttpPost("ListSlide")]
        public List<SlideModel.Output.SlideInfo> ListSlide(SlideModel.Input.ListSlide input)
        {
            var slides = _iSlide.ListSlide(input.Manage);
            var listSlide = slides.Select(x => new SlideModel.Output.SlideInfo
            {
                Id = x.Id,
                Name = x.Name,
                Active = x.Active,
                Url = x.Url,
                Picture = x.Picture
            }).ToList();
            return listSlide;
        }

        [HttpPost("Detail/{productId}")]
        public IActionResult SlideInfo(int productId)
        {
            var slide = _iSlide.SlideInfo(productId);

            return Ok(slide);
        }

        [HttpPost("CreateSlide")]
        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] SlideModel.Output.CreateSlide input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var slideId = _iSlide.Create(input);
            if (slideId == 0)
            {
                return BadRequest();
            }
            return Ok(slideId);
        }

        [HttpPost("UpdateSlide")]
        [Consumes("multipart/form-data")]
        public IActionResult Update([FromForm] SlideModel.Output.UpdateSlide input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var slideId = _iSlide.Update(input);
            if (slideId == 0)
            {
                return BadRequest();
            }
            return Ok(slideId);
        }

        [HttpPost("DeleteSlide")]
        public IActionResult Delete(SlideModel.Input.DeleteSlide input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var code = _iSlide.Delete(input);
            if (code == 0)
            {
                return BadRequest();
            }
            return Ok(code);
        }
    }
}