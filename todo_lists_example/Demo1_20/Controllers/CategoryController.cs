using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Demo1_20.Models;
using System.IO;
using System.Web.Mvc;

namespace Demo1_20.Controllers
{
    public class CategoryController : Controller
    {
        public static List<CategoryModel> CategoriesInList = new List<CategoryModel>();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string catID, string catName)
        {
            CategoryModel NewCat = new CategoryModel();

            NewCat.catID = Convert.ToInt32(catID);

            NewCat.catName = catName;

            CategoriesInList.Add(NewCat);

            return View(CategoriesInList.ToList());

            // TempData["Message"] = "Success";


            // WritechangesTofile();

            // TempData["Categories"] = CategoriesInList;
            // return RedirectToAction("SeedCat", "List");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           var category = CategoriesInList.Find(x => x.catID == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.catID = category.catID;
            ViewBag.catName = category.catName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string catID, string catName)
        {
                var changedCategory = CategoriesInList.Find(x => x.catID ==Convert.ToInt32(catID));
                changedCategory.catName = catName;
                TempData["Message"] = "The Category was successfully changed to " + "--" + "" + catName + "" + "--" + "!!";
                WritechangesTofile();
                return RedirectToAction("SeedCat", "List");
        }

        public ActionResult Delete(int id)
        {
            var Category = CategoriesInList.Find(x => x.catID == id);
            CategoriesInList.Remove(Category);
            TempData["Message"] = "The Category" + "" + "--" + "" + Category.catName + "" + "--" + "was successfully deleted";
            WritechangesTofile();
            return RedirectToAction("SeedCat", "List");
        }

        public ActionResult WritechangesTofile()
        {
            ////Create path locations for both category and Items

            string categoryfilepath = Server.MapPath("~/CreatedTextFiles/");

            ////////use MVC System.IO to write files to text folder created name Crteated Text Files on the right

            using (StreamWriter outputFile1 = new StreamWriter(Path.Combine(categoryfilepath, "Categories.txt")))
            {
                foreach (var category in CategoriesInList)//loop through each category in list
                    outputFile1.WriteLine(Convert.ToString(category.catID) + "," + Convert.ToString(category.catName));
            }

            return RedirectToAction("Index", "List");
        }

    }
}