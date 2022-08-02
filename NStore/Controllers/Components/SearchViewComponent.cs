using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Controllers.Components
{
    public class SearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = Utilities.SendDataRequest<List<CategoryModel>>(ConstantValues.Category.GetAll);
            categories = categories.Where(x => x.ParentId.Equals(null)).ToList();
            TempData["CategoryParentId"] = ViewBag.CategoryParentId;
            return View(categories);
        }
    }
}