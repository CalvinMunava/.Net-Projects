using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReportingExample.Models;


namespace ReportingExample.Controllers
{
    public class HomeController : Controller
    {
        private ReportStoreEntities db = new ReportStoreEntities();

        public ActionResult Index(int? productID)
        {
            //Get List of Names to load to view
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");

            //check if productID is null try find the users
            if (productID != null)
            {
                ViewBag.SelectedID = productID;

                var InvoiceLinesFound = db.InvoiceLines.Include(c => c.Invoice).Include( c => c.Product).Where(x => x.ProductID == productID).ToList();
                return View(InvoiceLinesFound);
            }
            else
            {               
                return View();
            }
        }

        public JsonResult GetChartData(int quantity)
        {
            //Instantiatelist to hold data
            List<ProductHolder> holdinglist = new List<ProductHolder>();

            //Group Objects
            var ProductsQuery = (from invoiceline in db.InvoiceLines
                            group invoiceline by invoiceline.ProductID into groupedobjects
                            orderby groupedobjects.Key
                            select new ProductHolder
                            {
                                ProductID = groupedobjects.Key,
                                ProductName = groupedobjects.FirstOrDefault().Product.ProductName,
                                ProductQuantity = (int)groupedobjects.Sum(oi => oi.Quantity),
                            }).Where(x=> x.ProductQuantity >= quantity).ToList();


            var ProductsLamda = db.InvoiceLines.GroupBy(x => x.ProductID).Select(x => new ProductHolder
                            {
                                ProductID = x.Key,
                                ProductName = x.FirstOrDefault().Product.ProductName,
                                ProductQuantity = (int)x.Sum(oi => oi.Quantity),
                            }).Where(x => x.ProductQuantity >= quantity).ToList();


            return new JsonResult { Data = ProductsQuery,  JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
   
    }
}