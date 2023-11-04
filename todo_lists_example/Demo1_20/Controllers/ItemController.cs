using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Demo1_20.Models;

namespace Demo1_20.Controllers
{
    public class ItemController : Controller
    {
        public static List<ItemModel> ItemsInList = new List<ItemModel>();

        public ActionResult Create(int? id)
        {
            ViewBag.catID = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1(string catID, string itemID, string itemName)
        {
            ItemModel newItem = new ItemModel(); 
            newItem.catID = Convert.ToInt32(catID);
            newItem.itemName = itemName;
            newItem.itemID = Convert.ToInt32(itemID);
            newItem.Completed = false;
            ItemsInList.Add(newItem);
            WritechangesTofile();
            TempData["Message"] = "Success";
            TempData["NewItem"] = newItem;
            return RedirectToAction("SeedItem", "List");
        }

        public ActionResult Edit(int? id, int catID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = ItemsInList.Find(x => x.itemID == id && x.catID == catID);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Name = item.itemName;
            ViewBag.catID = item.itemID;
            ViewBag.itemID = item.catID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(int itemID, int catID , string itemName)
        {
            var changedItem = ItemsInList.Find(x => x.itemID == itemID && x.catID == catID);
            changedItem.itemName = itemName;
            TempData["Message"] = "The Item was successfully changed to " + "--" + "" + itemName + "" + "--" + "!!";
            WritechangesTofile();
            return RedirectToAction("Index", "List");
        }


        public ActionResult Delete(int id, int catID)
        {
            var Item = ItemsInList.Find(x => x.itemID == id && x.catID == catID);
            ItemsInList.Remove(Item);
            TempData["Message"] = "The Item" + "" + "--" + "" + Item.itemName + "" + "--" + "was successfully deleted";
            TempData["RemovedItem"] = Item;
            WritechangesTofile();
            return RedirectToAction("SeedRemoved", "List");

        }


        public ActionResult DeleteChecked()
        {
            List<ItemModel> checkedItems = ItemsInList.Where(selc => selc.Completed == true).ToList();
            foreach (var item in checkedItems)
            {
                ItemsInList.Remove(item);
            }
            WritechangesTofile();
            TempData["Message"] = "The Completed Items have been Removed";
            TempData["RemovedItems"] = checkedItems;
            return RedirectToAction("SeedRemoved", "List");
        }


        public ActionResult Togglechecked(int id, int catID)
        {
            var Toggleitem = ItemsInList.Find(x => x.itemID == id && x.catID == catID );

            if (Toggleitem.Completed == false)
            {
                Toggleitem.Completed = true;
            }
            else
            {
                Toggleitem.Completed = false;
            }
            WritechangesTofile();
            return RedirectToAction("SeedCat", "List");
        }


        public ActionResult WritechangesTofile()
        {
            string itemfilepath = Server.MapPath("~/CreatedTextFiles/");
            using (StreamWriter outputFile2 = new StreamWriter(Path.Combine(itemfilepath, "Items.txt")))
            {
                foreach (var item in ItemsInList)//loop through each item in list
                outputFile2.WriteLine(Convert.ToString(item.itemName) + "," + Convert.ToString(item.itemID) + "," + Convert.ToString(item.catID) + "," + Convert.ToString(item.Completed));
            }

            return RedirectToAction("Index", "List");
        }

    }
}