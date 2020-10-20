using BusinessLayer;
using BusinessLayer.Entities;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private ProductService _productService;
        public ProductController()
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


        
        public ViewResult List(int? Id = null)
        {
            if(Id.Equals(null))
                return View(_productService.GetAllProducts());
            return View(_productService.GetProductsByCategoryId(Id.Value));
        }

        public ViewResult SelectProduct(int? Id, string returnUrl)
        {
            if (Id.Equals(null))
                RedirectToAction("DataNotFound","Error", new { message = "Выбран несуществующий товар"});
            //throw new Exception("mdaaaaaaa");
            return View( new ProductViewModel() {
                Product = _productService.GetProductById(Id.Value),
                ReturnUrl = returnUrl});
        }

      

        public PartialViewResult Menu()
        {
            return PartialView(_productService.GetCategories());
        }



        private void AdjustDataDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));// todo
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