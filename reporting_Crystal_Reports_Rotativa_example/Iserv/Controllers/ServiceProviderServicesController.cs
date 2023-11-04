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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iserv.Controllers
{
    public class ServiceProviderServicesController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        ApplicationDbContext context;

        public ServiceProviderServicesController()
        {
            context = new ApplicationDbContext();
        }

        public ServiceProviderServicesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: ServiceProviderServices
        public async Task<ActionResult> Index()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            //fnd user in ser table
          ServiceProvider userIn = db.ServiceProviders.Where(userr => userr.UserID == userInUser.UserID).FirstOrDefault();

            var serviceProviderServices = db.ServiceProviderServices.Include(s => s.Category).Include(s => s.Service).Include(s => s.ServiceProvider).Where(X => X.ServiceProvider_ID == userIn.ServiceProvider_ID);
            return View(await serviceProviderServices.ToListAsync());
        }

        // GET: ServiceProviderServices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceProviderService serviceProviderService = await db.ServiceProviderServices.FindAsync(id);
            if (serviceProviderService == null)
            {
                return HttpNotFound();
            }
            return View(serviceProviderService);
        }

        // GET: ServiceProviderServices/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            ViewBag.ServiceProvider_ID = new SelectList(db.ServiceProviders, "ServiceProvider_ID", "Name");
            return View();
        }

        // POST: ServiceProviderServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServiceProviderServiceID,ServiceProvider_ID,ServiceID,CategoryID")] ServiceProviderService serviceProviderService)
        {
            if (ModelState.IsValid)
            {
                db.ServiceProviderServices.Add(serviceProviderService);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", serviceProviderService.CategoryID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", serviceProviderService.ServiceID);
            ViewBag.ServiceProvider_ID = new SelectList(db.ServiceProviders, "ServiceProvider_ID", "Name", serviceProviderService.ServiceProvider_ID);
            return View(serviceProviderService);
        }

        // GET: ServiceProviderServices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceProviderService serviceProviderService = await db.ServiceProviderServices.FindAsync(id);
            if (serviceProviderService == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", serviceProviderService.CategoryID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", serviceProviderService.ServiceID);
            ViewBag.ServiceProvider_ID = new SelectList(db.ServiceProviders, "ServiceProvider_ID", "Name", serviceProviderService.ServiceProvider_ID);
            return View(serviceProviderService);
        }

        // POST: ServiceProviderServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceProviderServiceID,ServiceProvider_ID,ServiceID,CategoryID")] ServiceProviderService serviceProviderService)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceProviderService).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", serviceProviderService.CategoryID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", serviceProviderService.ServiceID);
            ViewBag.ServiceProvider_ID = new SelectList(db.ServiceProviders, "ServiceProvider_ID", "Name", serviceProviderService.ServiceProvider_ID);
            return View(serviceProviderService);
        }

        // GET: ServiceProviderServices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceProviderService serviceProviderService = await db.ServiceProviderServices.FindAsync(id);
            if (serviceProviderService == null)
            {
                return HttpNotFound();
            }
            return View(serviceProviderService);
        }

        // POST: ServiceProviderServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceProviderService serviceProviderService = await db.ServiceProviderServices.FindAsync(id);
            db.ServiceProviderServices.Remove(serviceProviderService);
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
