using EagerSample.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EagerSample.Controllers
    {
    public class SearchController : Controller
        {
        private NorthwindDB db = new NorthwindDB();

        public ActionResult Index()
            {
            return View(new SearchModel());
            }

        [HttpPost]
        public ActionResult Index(SearchModel model)
            {
            switch (model.SelectedCriteria)
                {
                // Customer ID
                // ------------------------------------
                case "Customer ID":
                    model.Results = db.Customers.Include(c => c.Orders.Select(o => o.Order_Details.Select(od => od.Products))).Where(c => c.CustomerID.Contains(model.SearchText)).ToList();
                    break;

                // Company Name
                // ------------------------------------
                case "Company Name":
                    model.Results = db.Customers.Include(c => c.Orders.Select(o => o.Order_Details.Select(od => od.Products))).Where(c => c.CompanyName.Contains(model.SearchText)).ToList();
                    break;

                // Contact Name
                // ------------------------------------
                case "Contact Name":
                    model.Results = db.Customers.Include(c => c.Orders.Select(o => o.Order_Details.Select(od => od.Products))).Where(c => c.ContactName.Contains(model.SearchText)).ToList();
                    break;
                }
            return View(model);
            }

        private bool IsValidInput(string input)
            {
            return !string.IsNullOrEmpty(input);
            }
        }
    }