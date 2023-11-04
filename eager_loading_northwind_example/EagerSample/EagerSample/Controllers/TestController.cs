using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EagerSample.Models;


namespace EagerSample.Controllers
{
    public class TestController : Controller
    {



        // GET : Test
        public ActionResult Index()
        {
            // SESSION 1
            List<TyraBanks> tyraBanks = new List<TyraBanks>(); // 100
            List<string> list = new List<string>();
            List<int> list2 = new List<int>();
            List<object> list3 = new List<object>();

            // Syntax
            var listTyra = tyraBanks.ToList();
            List<TyraBanks> listTyra1 = tyraBanks.ToList();

            TyraBanks tyra = listTyra.Where(anything => anything.tyraId == 1).FirstOrDefault();


            // Last 20 objects in list 
            var last20 = tyraBanks.Skip(80).ToList();


            // Select into a new model object 
            var TyraCounts = tyraBanks.Select(x =>
                                                new TyraCounts
                                                {
                                                    count = 1,
                                                    tyraId = x.tyraId

                                                }).ToList();

            // Select between 80-87
            var between80and87 = tyraBanks.Skip(79).Take(8).ToList();
            ViewBag.Error = "What am i gonna write here ";
            TempData["Error"] = "What am i gonna write here ";
            @Html.ActionLink("Edit", "Edit", new { id = item.ProductID, @class = "btn btn-danger buttons" }) |
            // SESSION 1


            return RedirectToAction("Index2");
        }


        public ActionResult Index2()
        {
            ViewBag.Error = TempData["Error"];
            return View();


        }



    }
}