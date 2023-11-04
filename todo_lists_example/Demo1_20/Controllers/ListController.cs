using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo1_20.Models;
using System.IO;
using System.Dynamic;


namespace Demo1_20.Controllers
{
    public class ListController : Controller
    {

        public static List<CategoryModel> categories = new List<CategoryModel>();
       
        [HttpGet]
        public ActionResult Index()
        {
            CategoryModel cat = new CategoryModel();
            var Msg = TempData["Message"] as string;

            if (Msg == null || Msg == "")
            {
                ViewBag.Message = "Welcome";
                ViewBag.name = cat.catName;
            }
            else
            {
                ViewBag.Message = Msg;
            }
            return View(categories.ToList());
        }

        public ActionResult SeedCat()
        {
            var newlist = TempData["Categories"] as List<CategoryModel>;
            categories = newlist;
            var Msg = TempData["Message"] as string;
            return RedirectToAction("Index", "List");
        }

        public ActionResult SeedItem()
        {
            var newAddedItem = TempData["NewItem"] as ItemModel;
            var ActualCategory = categories.Find(x => x.catID == newAddedItem.catID);
            ActualCategory.addItem(newAddedItem);
            return RedirectToAction("Index", "List");
        }



        public ActionResult SeedRemoved()
        {
            var removedItem = TempData["RemovedItems"] as List<ItemModel>;
            foreach( var removeI in removedItem)
            {
                var RemoveFromCategory = categories.Find(x => x.catID == removeI.catID);
                RemoveFromCategory.removeItem(removeI);
            }        
            return RedirectToAction("SeedCat", "List");
        }
    }
}