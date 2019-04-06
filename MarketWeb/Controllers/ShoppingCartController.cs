using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MarketWeb.Models;
using System;
using MarketWeb.Models.ViewModels;

namespace MarketWeb.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IHttpContextAccessor _contextAccesor;

        private string strCart = "Cart";

        private List<Cart> IsCart;


        public ShoppingCartController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccesor = contextAccessor;

        }


        public IActionResult Index()
        {
            return View();
        }


        
        public IActionResult OrderNow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_contextAccesor.HttpContext.Session.GetString(strCart) == null)
            {
                IsCart = new List<Cart>
                {
                    new Cart(_context.Products.Find(id), 1)
                };

                _contextAccesor.HttpContext.Session.SetObjectAsJson(strCart, IsCart);
            }

            else
            {
                IsCart = _contextAccesor.HttpContext.Session.GetObjectFromJson<List<Cart>>(strCart);

                int check = IsExistingCheck(id);

                if (check == -1)
                {
                    IsCart.Add(new Cart(_context.Products.Find(id), 1));
                }
                else
                {
                    IsCart[check].Quantity++;
                }

                _contextAccesor.HttpContext.Session.SetObjectAsJson(strCart, IsCart);
            }
            return View("Index");
        }


        private int IsExistingCheck(int? id)
        {
            IsCart = _contextAccesor.HttpContext.Session.GetObjectFromJson<List<Cart>>(strCart);

            for (int i = 0; i < IsCart.Count; i++)
            {
                if (IsCart[i].Product.ProductID == id) return i;
            }

            return -1;
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IsCart = _contextAccesor.HttpContext.Session.GetObjectFromJson<List<Cart>>(strCart);

            int check = IsExistingCheck(id);


            IsCart.RemoveAt(check);

            _contextAccesor.HttpContext.Session.SetObjectAsJson(strCart, IsCart);

            return RedirectToAction("Index");
        }


        public IActionResult UpdateCart()
        {
            string[] quantities = _contextAccesor.HttpContext.Request.Form["quantity"];

            IsCart = _contextAccesor.HttpContext.Session.GetObjectFromJson<List<Cart>>(strCart);

            for (int i = 0; i < IsCart.Count; i++)
            {
                IsCart[i].Quantity = Convert.ToInt32(quantities[i]);
            }

            _contextAccesor.HttpContext.Session.SetObjectAsJson(strCart, IsCart);

            return RedirectToAction("Index");
        }


        public ActionResult CheckOut()
        {

            return View("CheckOut");
        }

        public ActionResult ProcessOrder(IFormCollection frc)
        {
            IsCart = _contextAccesor.HttpContext.Session.GetObjectFromJson<List<Cart>>(strCart);

            Order order = new Order()
            {
                CustomerName = frc["cusName"],
                CustomerPhone = frc["cusPhone"],
                CustomerEmail = frc["cusEmail"],
                CustomerAddress = frc["cusAddress"],
                OrderDate = DateTime.Now,
                PaymentType = "Cash",
                Status = "Processing"
            };
            _context.Orders.Add(order);
            _context.SaveChanges();


            foreach (var cart in IsCart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderID = order.OrderID,
                    ProductID = cart.Product.ProductID,
                    Quantity = cart.Quantity,
                    Price = cart.Product.Price
                };
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
            }


            _contextAccesor.HttpContext.Session.Remove(strCart);

            return View("OrderSuccess");
        }
    }
}