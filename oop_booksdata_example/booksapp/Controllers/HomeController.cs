using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using booksapp.Models;

namespace booksapp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(listRepository.FakeData());
        }
    }
}