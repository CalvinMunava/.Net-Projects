using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Iserv.Models;
using System.IO;
using System.Web.Services;

namespace Iserv.Controllers
{
    public class ServiceQuotesController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: ServiceQuotes
        public async Task<ActionResult> Index()
        {
            var serviceQuotes = db.ServiceQuotes.Include(s => s.PaymentMethod);
            return View(await serviceQuotes.ToListAsync());
        }


        public async Task<ActionResult> IndexCustomer()
        {
            //recive current login 

            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();
      
            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            //fnd user in ser table
            Consumer userInConsumer = db.Consumers.Where(userr => userr.UserID == userInUser.UserID).FirstOrDefault();


            var serviceQuotes = db.ServiceQuotes.Include(s => s.PaymentMethod).Where(x => x.ConsumerID == userInConsumer.ConsumerID);
            return View(await serviceQuotes.ToListAsync());
        }


        public async Task<ActionResult> IndexProviders()
        {
            //recive current login 

            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            //find consumer 
            ServiceProvider userInProvider = db.ServiceProviders.Where(userr => userr.Name == userInUser.UserName).FirstOrDefault();


            var serviceQuotes = db.ServiceQuotes.Include(s => s.PaymentMethod).Where(x => x.ServiceProvider_ID == userInProvider.ServiceProvider_ID );

            return View(await serviceQuotes.ToListAsync());
        }


        // GET: ServiceQuotes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceQuote serviceQuote = await db.ServiceQuotes.FindAsync(id);
            if (serviceQuote == null)
            {
                return HttpNotFound();
            }
            return View(serviceQuote);
        }

        // GET: ServiceQuotes/Details/5
        public async Task<ActionResult> PastDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceQuote serviceQuote = await db.ServiceQuotes.FindAsync(id);
            if (serviceQuote == null)
            {
                return HttpNotFound();
            }
            return View(serviceQuote);
        }

        // GET: ServiceQuotes/Create
        public async Task<ActionResult> Create(int id)
        {
            //find service request change its status to accepted
            var serviceRequest = db.ServiceRequestLines.Include(s => s.ServiceRequest).Where( x=> x.ServiceRequestID == id).FirstOrDefault();
            serviceRequest.ServiceRequest.ServiceRequest_Status_ID = 2;

            ViewBag.ConsumerName = serviceRequest.ServiceRequest.Consumer.Name;
            ViewBag.Businessname = serviceRequest.ServiceProvider.BusinessName;
            ViewBag.Address = serviceRequest.ServiceProvider.Address.StreetNumber + " " + serviceRequest.ServiceProvider.Address.StreetName + " ," + serviceRequest.ServiceProvider.Address.City.Name;         
            ViewBag.ServiceRequest = serviceRequest.ServiceRequestID;

            //Create Quotation HERE
            ServiceQuote newQuote = new ServiceQuote();
            newQuote.ConsumerID = serviceRequest.ServiceRequest.ConsumerID;
            newQuote.ServiceProvider_ID = serviceRequest.ServiceProvider_ID;
            newQuote.ServiceQuote_Status_ID = 1;
            newQuote.ServiceRequestID = serviceRequest.ServiceRequestID;
            db.ServiceQuotes.Add(newQuote);
            db.Entry(serviceRequest).State = EntityState.Modified;
            await  db.SaveChangesAsync();

            //find quotation
            var quoteID = db.ServiceQuotes.Where(x => x.ServiceProvider_ID == serviceRequest.ServiceProvider_ID && x.ConsumerID == serviceRequest.ServiceRequest.ConsumerID && x.ServiceQuote_Status_ID == 1).FirstOrDefault();
            ViewBag.QuoteID = quoteID.ServiceQuoteID;

            ViewBag.PaymentMethod_ID = new SelectList(db.PaymentMethods, "PaymentMethodID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Create(string quotatonNumber, string Total , string PaymentMenthod_ID , string Validuntil)
        {
            var resu = true;
           int quotID = Convert.ToInt32(quotatonNumber);
           double TotalFixed = Convert.ToDouble(Total);
           int PaymentFixed= Convert.ToInt32(PaymentMenthod_ID);
           string Valid = DateTime.Parse(Validuntil).ToShortDateString();
            DateTime Date = DateTime.Parse(Valid);

            //find quote
            var serviceQuotefound = db.ServiceQuotes.Where(x => x.ServiceQuoteID == quotID).FirstOrDefault();
            serviceQuotefound.Total = TotalFixed;
            serviceQuotefound.PaymentMethod_ID = PaymentFixed;
            serviceQuotefound.ValidUntil = Date;
            serviceQuotefound.ServiceQuote_Status_ID = 2;
            db.Entry(serviceQuotefound).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return new JsonResult { Data = resu, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: ServiceQuotes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceQuote serviceQuote = await db.ServiceQuotes.FindAsync(id);
            if (serviceQuote == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentMethod_ID = new SelectList(db.PaymentMethods, "PaymentMethodID", "Name", serviceQuote.PaymentMethod_ID);
            return View(serviceQuote);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceQuoteID,Item,Price,PaymentMethod_ID")] ServiceQuote serviceQuote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceQuote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentMethod_ID = new SelectList(db.PaymentMethods, "PaymentMethodID", "Name", serviceQuote.PaymentMethod_ID);
            return View(serviceQuote);
        }

        // GET: ServiceQuotes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceQuote serviceQuote = await db.ServiceQuotes.FindAsync(id);
            if (serviceQuote == null)
            {
                return HttpNotFound();
            }
            return View(serviceQuote);
        }

        // POST: ServiceQuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceQuote serviceQuote = await db.ServiceQuotes.FindAsync(id);
            db.ServiceQuotes.Remove(serviceQuote);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //---------------------------------------------------Added-----------------------------------------------\\
        [WebMethod]
        [HttpPost]
        public JsonResult AddLines(int Quotation_ID, List<string> Descriptions, List<int> Quantaties, List<decimal> Prices, List<decimal> Totals)
        {

            using ( db)
            {
                for (int a = 0; a <= Totals.Count() - 1; a++)
                {
                    ServiceQuoteLine current = new ServiceQuoteLine();
                    current.ServiceQuoteID = Quotation_ID;
                    current.ItemDescription= Descriptions[a];
                    current.Quantity = Quantaties[a];
                    current.ItemPrice = Convert.ToDecimal(Prices[a]);
                    current.ItemTotal = Convert.ToDecimal(Totals[a]);
                    db.ServiceQuoteLines.Add(current);
                    db.SaveChanges();
                    db.Configuration.ProxyCreationEnabled = false;
                }
            }

            var Added = "Success";
            return new JsonResult { Data = Added, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



    }
}
