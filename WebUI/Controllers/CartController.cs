using BusinessLayer;
using BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart

        private ProductService _productService;

        public CartController()
        {
            try
            {
                _productService = new ProductService(GetConnectionStringToFile());
            }
            catch (Exception ex)
            {
                RedirectToAction("CannotConnectToDataBase", "Error", new { message = ex.Message });
            }
        }

        public ViewResult CartView(string returnUrl)
        {
            return View(
                new CartViewModel
                {
                    Cart = GetCart(),
                    ReturnUrl = returnUrl
                }
                );
        }

        public RedirectResult AddToCart(int prodId, int qty = 1)
        {
            GetCart().AddItem(_productService.GetProductById(prodId), qty);
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        public RedirectResult RemoveFromCart(int prodId)
        {
            GetCart().RemoveLine(_productService.GetProductById(prodId));
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }


        public PartialViewResult CartBox()
        {
            return PartialView(GetCart());
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ViewResult MakeOrder()
        {
            return View(new ShippingModel());
        }

        [HttpPost]
        public ViewResult MakeOrder(ShippingModel shippingModel)
        {
            if(GetCart().Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                GetCart().Clear();
                return View("Completed");
            }
            else
                return View(shippingModel);
        }



        private void AdjustDataDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            //Console.WriteLine("path= {0}", absolute);
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }

        private string GetConnectionStringToFile()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionToFile"].ConnectionString;
            AdjustDataDirectory();
            return connectionString;
        }

        protected override void Dispose(bool disposing)
        {
            _productService.Dispose();
            base.Dispose(disposing);
        }
    }
}