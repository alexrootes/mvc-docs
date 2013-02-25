using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc_docs.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			return RedirectToAction("Index", "Documents", new { product = "example", language = "en", version = "latest" });
        }
    }
}
