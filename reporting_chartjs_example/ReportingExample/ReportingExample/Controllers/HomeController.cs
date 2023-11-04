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
            var Products = (from invoiceline in db.InvoiceLines
                            group invoiceline by invoiceline.ProductID into groupedobjects
                            orderby groupedobjects.Key
                            select new ProductHolder
                            {
                                ProductID = groupedobjects.Key,
                                ProductQuantity = (int)groupedobjects.Sum(q => q.Quantity)
                            }).ToList(); ;
            //loop through each object and add name to complete model
            // make list to hold things for chart
            List<ProductHolder> chartlist = new List<ProductHolder>();
            foreach (var line in Products)
            {
                var findProductName = db.Products.Where(x => x.ProductID == line.ProductID).FirstOrDefault();
                line.ProductName = findProductName.ProductName;
                if(line.ProductQuantity >= quantity)
                {
                    chartlist.Add(line);
                }
            }

            //render things for calander
            //List<int> quantities = new List<int>();
            //var productnames = chartlist.Select(x => x.ProductName).Distinct();
            //foreach( var chartItem in chartlist)
            //{
            //    quantities.Add(chartItem.ProductQuantity);
            //}
            // send data to back
    
            return new JsonResult { Data = chartlist,  JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}