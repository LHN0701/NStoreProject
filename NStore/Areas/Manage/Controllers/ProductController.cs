using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ProductController : Controller
    {
        public IActionResult Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var input = new ProductModel.Input.GetProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var data = Utilities.SendDataRequest<ProductModel.Output.PagedResult>(ConstantValues.Product.Paging, input);
            ViewBag.Keyword = keyword;

            var categories = Utilities.SendDataRequest<List<CategoryModel>>(ConstantValues.Category.GetAll);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });
            if (TempData["result"] != null)
            {
                ViewBag.Successmsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Detail(int productId)
        {
            var data = Utilities.SendDataRequest<ProductModel.ProductBase>(ConstantValues.Product.Detail + $"/{productId}", productId);
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Create(ProductModel.Output.AddProduct request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = ProductApiRequest.CreateProductRequest(request);
            if (result > 0)
            {
                TempData["result"] = "Create product success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Create product fail");
            return View(request);
        }

        [HttpGet]
        public IActionResult Update(int productId)
        {
            var product = Utilities.SendDataRequest<ProductModel.ProductBase>(ConstantValues.Product.Detail + $"/{productId}", productId);
            var updateProduct = new ProductModel.Output.UpdateProduct()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Description = product.Description,
                Details = product.Details,
                ImagePath = product.ImagePath
            };
            return View(updateProduct);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Update([FromForm] ProductModel.Output.UpdateProduct request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = ProductApiRequest.UpdateProductRequest(request);
            if (result > 0)
            {
                TempData["result"] = "Update product success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Update product fail");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(ProductModel.Input.DeleteProduct request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await ProductApiRequest.DeleteProduct(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Delete product success";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Noteti);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CategoryAssignRequest(int productId)
        {
            var product = Utilities.SendDataRequest<ProductModel.ProductBase>(ConstantValues.Product.Detail + $"/{productId}", productId);
            var categories = Utilities.SendDataRequest<List<CategoryModel>>(ConstantValues.Category.GetAll);

            var CategoryAssign = new ProductModel.Output.CategoryAssign()
            {
                Id = productId,
                Categories = categories.Select(x => new ProductModel.Input.CategoryAssign()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Selected = product.Categories.Contains(x.Name)
                }).ToList()
            };
            return View(CategoryAssign);
        }

        [HttpPost]
        public IActionResult CategoryAssign(int id, ProductModel.Output.CategoryAssign request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = Utilities.SendDataRequest<ProductModel.Output.CategoryAssign>(ConstantValues.Product.CategoryAssign + $"/{id}", request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật thể loại thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Noteti);
            var roleAssignRequest = CategoryAssignRequest(request.Id);

            return View(roleAssignRequest);
        }
    }
}