using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Auth;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _iProduct;

        public ProductsController(IProduct iProduct)
        {
            _iProduct = iProduct;
        }

        [HttpPost("Paging")]
        public IActionResult GetAllPaging(ProductModel.Input.GetProductPagingRequest request)
        {
            var result = _iProduct.GetAllPaging(request);
            return Ok(result);
        }

        [HttpPost("Detail/{productId}")]
        public IActionResult GetById(int productId)
        {
            var product = _iProduct.GetById(productId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpPost("ViewCount/{productId}")]
        public IActionResult ViewCount(int productId)
        {
            var result = _iProduct.ViewCount(productId);
            if (result > 0)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public IActionResult Create([FromForm] ProductModel.Output.AddProduct request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = _iProduct.Create(request);
            if (productId == 0)
                return BadRequest();

            return Ok(productId);
        }

        [HttpPost("update/{productId}")]
        [Consumes("multipart/form-data")]
        public IActionResult Update([FromRoute] int productId, [FromForm] ProductModel.Output.UpdateProduct request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var productid = _iProduct.Update(request);
            if (productid == 0)
                return BadRequest();
            return Ok(productid);
        }

        [HttpPost("delete/{productId}")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _iProduct.Detele(productId);

            return Ok(result);
        }

        [HttpPost("CategoryAssign/{id}")]
        public IActionResult CategoryAssign(int id, [FromBody] ProductModel.Output.CategoryAssign request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _iProduct.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("featured/{take}")]
        [AllowAnonymous]
        public IActionResult GetFeaturedProducts(int take)
        {
            var products = _iProduct.GetFeaturedProducts(take);
            return Ok(products);
        }

        [HttpPost("latest/{take}")]
        [AllowAnonymous]
        public IActionResult GetLastestProduct(int take)
        {
            var products = _iProduct.GetLatestProduct(take);
            return Ok(products);
        }
    }
}