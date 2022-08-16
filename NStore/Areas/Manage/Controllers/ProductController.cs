using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            var categories = Utilities.SendDataRequest<GetAllCategory>(ConstantValues.Category.GetAll);
            ViewBag.Categories = categories.CategoryParent.Select(x => new SelectListItem()
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
        public IActionResult CategoryAssignRequest(int productId, int? categoryId)
        {
            var CategoryAssign = new ProductModel.Output.CategoryAssign();

            var categories = Utilities.SendDataRequest<GetAllCategory>(ConstantValues.Category.GetAll);
            var productCategory = Utilities.SendDataRequest<CategoryModel>(ConstantValues.Product.ProductCategory + $"/{productId}", productId);

            if (productCategory.Id == 0 && categoryId == null)
            {
                CategoryAssign.Id = productId;
                CategoryAssign.ParentCategories = categories.CategoryParent.Select(x => new ProductModel.Input.CategoryAssign()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId
                }).ToList();
                return View(CategoryAssign);
            }

            TempData["CategoryParentId"] = productCategory.ParentId;
            var categoryChild = categories.CategoryChild.Where(x => x.ParentId.Equals(productCategory.ParentId));
            if (categoryId != null)
            {
                categoryChild = categories.CategoryChild.Where(x => x.ParentId.Equals(categoryId));
                TempData["CategoryParentId"] = categoryId;
            }

            CategoryAssign = new ProductModel.Output.CategoryAssign()
            {
                Id = productId,
                Categories = categoryChild
                    .Select(x => new ProductModel.Input.CategoryAssign()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentId = x.ParentId,
                        Selected = x.Id == productCategory.Id
                    }).ToList(),
                ParentCategories = categories.CategoryParent.Select(x => new ProductModel.Input.CategoryAssign()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId
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
            //var roleAssignRequest = CategoryAssignRequest(request.Id, null);

            return View(/*roleAssignRequest*/);
        }

        [HttpGet]
        public IActionResult GetAllImage(int productId)
        {
            var input = new ProductImageModel.Input.GetAllImageProduct()
            {
                ProductId = productId
            };

            if (TempData["result"] != null)
            {
                ViewBag.Successmsg = TempData["result"];
            }

            var listImages = Utilities.SendDataRequest<List<ProductImageModel.ProductImageBase>>(ConstantValues.Product.GetAllImage + $"/{productId}", input);

            return View(listImages);
        }

        [HttpGet]
        public IActionResult AddImage(int productId)
        {
            var model = new ProductImageModel.Output.AddImage()
            {
                ProductId = productId
            };
            return View(model);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult AddImage(ProductImageModel.Output.AddImage input)
        {
            if (!ModelState.IsValid)
                return View();

            var result = ProductApiRequest.AddImageRequest(input);

            if (result > 0)
            {
                TempData["result"] = "Create product success!";
                return RedirectToAction("GetAllImage", new ProductImageModel.Input.GetAllImageProduct { ProductId = result });
            }

            ModelState.AddModelError("", "Create product fail!");
            return View();
        }

        [HttpGet]
        public IActionResult UpdateImage(int imageId)
        {
            var image = Utilities.SendDataRequest<ProductImageModel.ProductImageBase>(ConstantValues.Product.GetImageById + $"/{imageId}", imageId);

            var updateImage = new ProductImageModel.Output.UpdateImage()
            {
                Id = image.Id,
                IsDefault = image.IsDefault,
                ImagePath = image.ImagePath,
                SortOrder = image.SortOrder,
                ProductId = image.ProductId
            };

            return View(updateImage);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult UpdateImage(ProductImageModel.Output.UpdateImage input)
        {
            if (!ModelState.IsValid)
                return View();

            var result = ProductApiRequest.UpdateImageRequest(input);

            if (result > 0)
            {
                TempData["result"] = "Update product success!";
                return RedirectToAction("GetAllImage", new ProductImageModel.Input.GetAllImageProduct { ProductId = result });
            }

            ModelState.AddModelError("", "Update product fail!");
            return View();
        }

        [HttpGet]
        public IActionResult DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
                return View();

            var result = Utilities.SendDataRequest<int>(ConstantValues.Product.DeleteImage + $"/{imageId}", imageId);
            if (result > 0)
            {
                TempData["result"] = "Delete image success!";
                return RedirectToAction("GetAllImage", new ProductImageModel.Input.GetAllImageProduct { ProductId = result });
            }

            ModelState.AddModelError("", "Delete image fail!");
            return View();
        }
    }
}