﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public IActionResult GetListItem()
        {
            var session = HttpContext.Session.GetString("CartItem");
            List<CartItemModel> currentCart = new();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemModel>>(session);
            }

            return Ok(currentCart);
        }

        public IActionResult AddToCart(int id, int? qty)
        {
            var product = Utilities.SendDataRequest<ProductModel.ProductBase>(ConstantValues.Product.Detail + $"/{id}", id);
            var session = HttpContext.Session.GetString("CartItem");
            List<CartItemModel> currentCart = new();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemModel>>(session);
            }

            var quantity = qty == null ? 1 : qty;

            if (currentCart.Any(x => x.ProductId == id) && qty == null)
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
                currentCart.Remove(currentCart.First(x => x.ProductId == id));
            }
            else if (currentCart.Any(x => x.ProductId == id) && qty != null)
            {
                quantity += currentCart.First(x => x.ProductId == id).Quantity;
                currentCart.Remove(currentCart.First(x => x.ProductId == id));
            }
            var cartItem = new CartItemModel()
            {
                ProductId = id,
                Name = product.Name,
                Description = product.Description,
                Image = product.ImagePath,
                Quantity = quantity.GetValueOrDefault(),
                Price = product.Price
            };

            currentCart.Add(cartItem);

            HttpContext.Session.SetString("CartItem", JsonConvert.SerializeObject(currentCart));

            if (qty != null)
                return RedirectToAction("index");

            return Ok(currentCart);
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString("CartItem");
            List<CartItemModel> currentCart = new();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemModel>>(session);

            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString("CartItem", JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }

        public IActionResult CheckOut()
        {
            return View(GetCheckoutViewModel());
        }

        [HttpPost]
        public IActionResult CheckOut(CheckOutModel request)
        {
            var memberId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "MemberId").Value);
            var model = GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailModel>();
            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
            var checkoutRequest = new CheckOutRequest()
            {
                UserId = memberId,
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                Email = request.CheckoutModel.Email,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails
            };
            //TODO: Add to API

            var result = Utilities.SendDataRequest<NotetiModel>(ConstantValues.Order.CreateOrder, checkoutRequest);

            if (result.Issuccess == false)
            {
                ModelState.AddModelError("", result.Noteti);
                return View();
            }

            TempData["SuccessMsg"] = result.Noteti;
            return View(model);
        }

        private CheckOutModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString("CartItem");
            List<CartItemModel> currentCart = new List<CartItemModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemModel>>(session);
            var checkout = new CheckOutModel()
            {
                CartItems = currentCart,
                CheckoutModel = new CheckOutRequest()
            };
            return checkout;
        }
    }
}