using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webCopa.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Homer()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Copa do Mundo 2018.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contato.";

            return View();
        }
    }
}