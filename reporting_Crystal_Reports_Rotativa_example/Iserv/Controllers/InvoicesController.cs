using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Iserv.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iserv.Controllers
{
    public class InvoicesController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();


        public async Task<ActionResult> IndexCustomers()
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


            var Invoices = db.Invoices.Include(i => i.Consumer).Include(i => i.ServiceProvider).Include(i => i.ServiceQuoteStatu).Include(i => i.ServiceRequest).Include(s => s.PaymentMethod).Where(x => x.ConsumerID == userInConsumer.ConsumerID);
            return View(await Invoices.ToListAsync());
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



            var Invoices = db.Invoices.Include(i => i.Consumer).Include(i => i.ServiceProvider).Include(i => i.ServiceQuoteStatu).Include(i => i.ServiceRequest).Include(s => s.PaymentMethod).Where(x => x.ServiceProvider_ID == userInProvider.ServiceProvider_ID);

            return View(await Invoices.ToListAsync());
        }


        public async Task<ActionResult> PastDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice serviceQuote = await db.Invoices.FindAsync(id);
            if (serviceQuote == null)
            {
                return HttpNotFound();
            }
            return View(serviceQuote);
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
