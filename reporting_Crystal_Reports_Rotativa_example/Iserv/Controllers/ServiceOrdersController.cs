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


namespace Iserv.Controllers
{
    public class ServiceOrdersController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: ServiceOrders
        public async Task<ActionResult> Index()
        {
            var serviceOrders = db.ServiceOrders.Include(s => s.ServiceOrder_Status).Include(s => s.ServiceQuote);
            return View(await serviceOrders.ToListAsync());
        }


        public async Task<ActionResult> IndexCustomers()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();


            //find consumer 
            Consumer userInConsumer = db.Consumers.Where(userr => userr.Name == userInUser.UserName).FirstOrDefault();


            var serviceOrders = db.ServiceOrders.Include(s => s.ServiceOrder_Status).Include(s => s.ServiceQuote).Where(x => x.ConsumerID == userInConsumer.ConsumerID );
            return View(await serviceOrders.ToListAsync());
        }

        public async Task<ActionResult> IndexProviders()
        {
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


            var serviceOrders = db.ServiceOrders.Include(s => s.ServiceOrder_Status).Include(s => s.ServiceQuote).Where(x => x.ServiceProvider_ID == userInProvider.ServiceProvider_ID);
            return View(await serviceOrders.ToListAsync());
        }

        // GET: ServiceOrders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrder serviceOrder = await db.ServiceOrders.FindAsync(id);
            if (serviceOrder == null)
            {
                return HttpNotFound();
            }
            return View(serviceOrder);
        }



        // GET: ServiceOrders/Create
        public async Task<ActionResult> Create(int id , string decision)
        {
            ServiceOrder serviceOrder = new ServiceOrder();
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            Consumer con = db.Consumers.Where(x => x.UserID == userInUser.UserID).FirstOrDefault();

            //find servicequote
            ServiceQuote serviceQuote = db.ServiceQuotes.Where(x => x.ServiceQuoteID == id).FirstOrDefault();

            //check the status 
            if (decision == "Accept")
            {

                //changes status of quote 
                serviceQuote.ServiceQuote_Status_ID = 3;
                db.Entry(serviceQuote).State = EntityState.Modified;
                await db.SaveChangesAsync();

                serviceOrder.Service_Order_Status_ID = 1;
                serviceOrder.ConsumerID = con.ConsumerID;
                serviceOrder.ServiceQuoteID = serviceQuote.ServiceQuoteID;
                serviceOrder.ServiceProvider_ID = serviceQuote.ServiceProvider_ID;
                db.ServiceOrders.Add(serviceOrder);
                await db.SaveChangesAsync();

                //create line object
                var serviceorder = db.ServiceOrders.Where(x => x.ServiceQuoteID == serviceQuote.ServiceQuoteID).FirstOrDefault();

                //first find quote
                var servicequotefound = db.ServiceQuotes.Where(x => x.ServiceQuoteID == serviceQuote.ServiceQuoteID).FirstOrDefault();

                var servicequotelinefound = db.ServiceQuoteLines.Where(x => x.ServiceQuoteID == serviceQuote.ServiceQuoteID).FirstOrDefault();

                //CreateLine object
                ServiceOrderLine newLine = new ServiceOrderLine();
                newLine.SericeOrderID = serviceorder.ServiceOrderID;
                newLine.ServiceQuote_ID = serviceQuote.ServiceQuoteID;
                newLine.ServiceQuoteLine_ID = servicequotelinefound.ServiceQuoteLine_ID;
                db.ServiceOrderLines.Add(newLine);
               await db.SaveChangesAsync();

                return RedirectToAction("IndexCustomers");
            }
            else if (decision == "Reject")
            {

                //changes status of quote 
                serviceQuote.ServiceQuote_Status_ID = 4;
                db.Entry(serviceQuote).State = EntityState.Modified;
                await db.SaveChangesAsync();


                serviceOrder.Service_Order_Status_ID = 2;
                serviceOrder.ConsumerID = con.ConsumerID;
                serviceOrder.ServiceQuoteID = serviceQuote.ServiceQuoteID;
                serviceOrder.ServiceProvider_ID = serviceQuote.ServiceProvider_ID;
                db.ServiceOrders.Add(serviceOrder);
                await db.SaveChangesAsync();
              
               return RedirectToAction("IndexCustomers");
            }
            return RedirectToAction("IndexCustomers");
        }


        public async Task<ActionResult> Complete(int? id, int? servicequote)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrder serviceOrder = await db.ServiceOrders.FindAsync(id);

            if (serviceOrder == null)
            {
                return HttpNotFound();
            }

            serviceOrder.Service_Order_Status_ID = 3;
            serviceOrder.datecompleted = DateTime.Now;
            db.Entry(serviceOrder).State = EntityState.Modified;
            await db.SaveChangesAsync();


            //find service quote
            var foundservicequote = db.ServiceQuotes.Where(x => x.ServiceQuoteID == servicequote).FirstOrDefault();

            //make invoice 
            Invoice invoice = new Invoice();
            invoice.Total = foundservicequote.Total;
            invoice.PaymentMethod_ID = foundservicequote.PaymentMethod_ID;
            invoice.ServiceQuote_Status_ID = foundservicequote.ServiceQuote_Status_ID;
            invoice.ConsumerID = foundservicequote.ConsumerID;
            invoice.ServiceProvider_ID = foundservicequote.ServiceProvider_ID;
            invoice.ServiceRequestID = foundservicequote.ServiceRequestID;
            invoice.ValidUntil = foundservicequote.ValidUntil;
            invoice.ServiceQuoteID = foundservicequote.ServiceQuoteID;
            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();
            return RedirectToAction("IndexProviders");



        }








        // GET: ServiceOrders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrder serviceOrder = await db.ServiceOrders.FindAsync(id);
            if (serviceOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.Service_Order_Status_ID = new SelectList(db.ServiceOrder_Status, "Service_Order_Status_ID", "Name", serviceOrder.Service_Order_Status_ID);
            
            return View(serviceOrder);
        }

        // POST: ServiceOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceOrderID,Service_Order_Status_ID,ServiceQuoteID")] ServiceOrder serviceOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceOrder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("IndexProviders");
            }
            ViewBag.Service_Order_Status_ID = new SelectList(db.ServiceOrder_Status, "Service_Order_Status_ID", "Name", serviceOrder.Service_Order_Status_ID);
            ViewBag.ServiceQuoteID = new SelectList(db.ServiceQuotes, "ServiceQuoteID", "Item", serviceOrder.ServiceQuoteID);
            return View(serviceOrder);
        }

        // GET: ServiceOrders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceOrder serviceOrder = await db.ServiceOrders.FindAsync(id);
            if (serviceOrder == null)
            {
                return HttpNotFound();
            }
            return View(serviceOrder);
        }

        // POST: ServiceOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceOrder serviceOrder = await db.ServiceOrders.FindAsync(id);
            db.ServiceOrders.Remove(serviceOrder);
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
    }
}
