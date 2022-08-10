using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NStore.ApiRequest;
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
    [Authorize("Admin")]
    public class SlideController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlideController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public IActionResult Index()
        {
            var input = new SlideModel.Input.ListSlide { Manage = true };
            var dsBanner = Utilities.SendDataRequest<List<SlideModel.Output.SlideInfo>>(ConstantValues.Slide.ListSlide, input);
            if (TempData["result"] != null)
            {
                ViewBag.Successmsg = TempData["result"];
            }
            return View(dsBanner);
        }

        [HttpGet]
        public IActionResult CreateSlide()
        {
            var slide = new SlideModel.Output.CreateSlide();
            return View(slide);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult CreateSlide([FromForm] SlideModel.Output.CreateSlide request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = SlideApiRequest.CreateSlideRequest(request);
            if (result > 0)
            {
                TempData["result"] = "Create slide success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Create slide fail");
            return View(request);
        }

        [HttpGet]
        public IActionResult UpdateSlide(int id)
        {
            var slide = Utilities.SendDataRequest<SlideModel.Output.SlideInfo>(ConstantValues.Slide.SlideInfo + $"/{id}", id);
            var updateSlide = new SlideModel.Output.UpdateSlide()
            {
                Id = slide.Id,
                Name = slide.Name,
                Picture = slide.Picture,
                Url = slide.Url,
                Active = slide.Active
            };
            return View(updateSlide);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult UpdateSlide(SlideModel.Output.UpdateSlide input)
        {
            if (!ModelState.IsValid)
                return View();
            var result = SlideApiRequest.UpdateSlideRequest(input);
            if (result > 0)
            {
                TempData["result"] = "Update slide success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Update slide fail");
            return View(input);
        }

        [HttpGet]
        public IActionResult DeleteSlide(SlideModel.Input.DeleteSlide input)
        {
            var result = Utilities.SendDataRequest<int>(ConstantValues.Slide.DeleteSlide, input);

            if (result > 0)
            {
                TempData["result"] = "Delete slide success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Delete slide fail");
            return RedirectToAction("Index");
        }
    }
}