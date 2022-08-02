using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NStore.Common;
using NStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStore.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            var product = Utilities.SendDataRequest<ProductModel.ProductBase>(ConstantValues.Product.Detail + $"/{id}", id);
            var session = HttpContext.Session.GetString("CartItem");
            List<CartItemModel> currentCart = new();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemModel>>(session);
            }

            var quantity = 1;
            if (currentCart.Any(x => x.ProductId == id))
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
            }

            var cartItem = new CartItemModel()
            {
                ProductId = id,
                Name = product.Name,
                Description = product.Description,
                Image = product.ImagePath,
                Quantity = quantity
            };

            currentCart.Add(cartItem);

            HttpContext.Session.SetString("CartItem", JsonConvert.SerializeObject(currentCart));

            return Ok();
        }
    }
}