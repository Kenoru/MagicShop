using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult DataNotFound(string message)
        {
            Response.StatusCode = 404;
            ViewBag.MyMessage = message;
            return View();
        }

        public ActionResult CannotConnectToDataBase(string message)
        {
            Response.StatusCode = 502;
            ViewBag.MyMessage = message;
            return View();
        }
    }
}