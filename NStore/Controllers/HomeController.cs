using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var input = new SlideModel.Input.ListSlide { Quantri = false };
            var data = new HomeModel
            {
                Slides = Utilities.SendDataRequest<List<SlideModel.Output.SlideInfo>>(ConstantValues.Home.Slides, input),
                FeaturedProducts = Utilities.SendDataRequest<List<ProductModel.ProductBase>>(ConstantValues.Home.FeatureProduct + $"/{6}", 6),
                LatestProducts = Utilities.SendDataRequest<List<ProductModel.ProductBase>>(ConstantValues.Home.LatestProduct + $"/{8}", 8)
            };

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}