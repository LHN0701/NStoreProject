using Microsoft.AspNetCore.Mvc;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Controllers.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = Utilities.SendDataRequest<GetAllCategory>(ConstantValues.Category.GetAll);
            return View(item);
        }
    }
}