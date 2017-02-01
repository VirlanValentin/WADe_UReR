using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Urer.Web.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Register()
    {
      ViewBag.Message = "Register Here";

      return View();
    }
    public ActionResult LoginQR()
    {
      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public ActionResult Home()
    {
      return View();
    }
    public ActionResult MyProfile()
    {
      return View();
    }
  }
}