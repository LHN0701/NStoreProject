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
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            if (TempData["result"] != null)
            {
                ViewBag.Successmsg = TempData["result"];
            }
            var listPromotion = Utilities.SendDataRequest<List<PromotionModel.PromotionBase>>(ConstantValues.Promotion.GetAll);
            return View(listPromotion);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PromotionModel.PromotionBase();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PromotionModel.PromotionBase request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Promotion.CreatePromotion, request);

            if (result.Issuccess)
            {
                TempData["result"] = result.Noteti;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Noteti);
            return View();
        }

        [HttpGet]
        public IActionResult Update(PromotionModel.Input.UpdatePromotion request)
        {
            var result = Utilities.SendDataRequest<PromotionModel.PromotionBase>(ConstantValues.Promotion.GetById + $"/{request.Id}", request.Id);

            return View(result);
        }

        [HttpPost]
        public IActionResult Update(PromotionModel.PromotionBase request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Promotion.UpdatePromotion, request);

            if (result.Issuccess)
            {
                TempData["result"] = result.Noteti;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Noteti);
            return View();
        }

        [HttpGet]
        public IActionResult Delete(PromotionModel.Input.DeletePromotion request)
        {
            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Promotion.DeletePromotion, request);

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