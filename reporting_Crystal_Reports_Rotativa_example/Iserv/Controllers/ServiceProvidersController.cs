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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Iserv.Controllers
{
    public class ServiceProvidersController : Controller
    {
        private ApplicationUserManager _userManager;
        private InternetServicesEntities db = new InternetServicesEntities();
        ApplicationDbContext context = new ApplicationDbContext();

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

        // GET: ServiceProviders
        public async Task<ActionResult> Index()
        {
            var serviceProviders = db.ServiceProviders.Include(s => s.Address).Include(s => s.User);
            return View(await serviceProviders.ToListAsync());
        }


        public async Task<ActionResult> IndexConsumer(string searchBy, string Businessname, string name)
        {
            if (searchBy == "Name")
            {
                var serviceProviders = db.ServiceProviders.Include(s => s.Address).Include(s => s.User).Where(x=> x.Name == name);
                return View(await serviceProviders.ToListAsync());
            }
            else if (searchBy == "BusinessName")
            {
                var serviceProviders = db.ServiceProviders.Include(s => s.Address).Include(s => s.User).Where(x => x.BusinessName == Businessname);
                return View(await serviceProviders.ToListAsync());
            }
            else
            {
                var serviceProviders = db.ServiceProviders.Include(s => s.Address).Include(s => s.User);
                return View(await serviceProviders.ToListAsync());
            }
         
        }



        // GET: ServiceProviders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceProvider serviceProvider = await db.ServiceProviders.FindAsync(id);
            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            return View(serviceProvider);
        }

        // GET: ServiceProviders/Create
        public ActionResult Create()
        {

            ViewBag.CityID = new SelectList(db.Cities.Select(c => new { c.CityID, c.Name }), "CityID", "Name");
            ViewBag.ProvinceID = new SelectList(db.Provinces.Select(c => new { c.ProvinceID, c.Name }), "ProvinceID", "Name");
            ViewBag.ServiceID = new SelectList(db.Services.Select(c => new { c.ServiceID, c.Name }), "ServiceID", "Name"); 
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Surname,BusinessName,ID_Number,Gender,ServiceID")] ServiceProvider serviceProvider , Address address , [Bind(Include = "UserName,Email, Password,ConfirmPassword")] RegisterViewModel model)
        {

 
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

                var result = await UserManager.CreateAsync(user, model.Password);

                var userStore = new UserStore<ApplicationUser>(context);

                var userManager = new UserManager<ApplicationUser>(userStore);

                var result1 = UserManager.AddToRole(user.Id, "ServiceProvider");
                await db.SaveChangesAsync();


                //first find ASPuSERbased offtheir username after creating them 
                var ASPtoFind = db.AspNetUsers.Where(x => x.UserName == model.UserName).FirstOrDefault();

                ////ADD THEM AS A USER 
                User newCreatedUser = new User();
                newCreatedUser.UserName = serviceProvider.Name;
                newCreatedUser.AspNetID = ASPtoFind.Id;
                newCreatedUser.Role = "ServiceProvider";
                db.Users.Add(newCreatedUser);
                await db.SaveChangesAsync();


                ////first find user based offtheir username after creating them 
                var UsertoFind = db.Users.Where(x => x.UserName == serviceProvider.Name).FirstOrDefault();


                address.UserID = UsertoFind.UserID;
                db.Addresses.Add(address);
                await db.SaveChangesAsync();

                ////first find consumer based offtheir username after creating them 
                var ProvidertoFind = db.Addresses.Where(x => x.UserID == UsertoFind.UserID).FirstOrDefault();

                serviceProvider.AverageRating = 5;
                serviceProvider.UserID = UsertoFind.UserID;
                serviceProvider.AddressID = ProvidertoFind.AddressID;
                db.ServiceProviders.Add(serviceProvider);
                await db.SaveChangesAsync();

                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Confirm your account", " Hi " + model.UserName + " Success !!! Service Provider Profile is Live :) \n\n Your Login Details Are: UserName -"+ " "+ model.UserName + " " + " Password: - " + " " + model.Password + "\n\n" + "Please Confirm Your Account By Clicking The Following Link : <a href=\"" + callbackUrl + "\">Confirm Account</a>");
                ViewBag.Link = callbackUrl;
                return RedirectToAction("ProviderComplete");
            }

            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", serviceProvider.AddressID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", serviceProvider.UserID);
            return View(serviceProvider);
        }


        public ActionResult ProviderComplete()
        {
            return View();
        }

        // GET: ServiceProviders/Edit/5
        public async Task<ActionResult> Edit()
        {

            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            ServiceProvider serviceProvider = await db.ServiceProviders.Where(x => x.UserID == userInUser.UserID).FirstOrDefaultAsync();


            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", serviceProvider.AddressID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", serviceProvider.UserID);
            return View(serviceProvider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Surname,BusinessName")] ServiceProvider serviceProvider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceProvider).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", serviceProvider.AddressID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", serviceProvider.UserID);
            return View(serviceProvider);
        }















        //---------------------------------------------------------Used This------------------------------------------------------------\\
        [HttpPost]
        public JsonResult AutoComplete(string prefix, string service)
        {
            var sp = (from servp in db.ServiceProviders
                      where servp.Name.StartsWith(prefix) 
                      select new
                      {
                          label = servp.Name,
                          val = servp.ServiceProvider_ID,
                      }).ToList();

            return Json(sp);
        }



        [HttpPost]
        public async Task<JsonResult> AddServiceProviderServicesAsync(int id, int serviceid, int categoryid)
        {

            ServiceProviderService newService = new ServiceProviderService();
            newService.ServiceProvider_ID = id;
            newService.ServiceID = serviceid;
            newService.CategoryID = categoryid;
            db.ServiceProviderServices.Add(newService);
            await db.SaveChangesAsync();

            db.Configuration.ProxyCreationEnabled = false;
            List<ServiceProviderService> Added = await db.ServiceProviderServices.Where(p => p.ServiceProvider_ID == id).ToListAsync();

            return new JsonResult { Data = Added, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<ActionResult> ManageServices(int? id, string name, string Pname)
        {
            ViewBag.ID = id;
            ViewBag.Name = Pname;
            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.UserID == id).FirstOrDefault();
            ServiceProvider serviceProvider = await db.ServiceProviders.Where(x => x.UserID == userInUser.UserID).FirstOrDefaultAsync();
            ViewBag.SID = serviceProvider.ServiceProvider_ID;
            //found provider now search service requests
            if (name == "" || name == null)
            {
                return View();
            }
            else
            {
                var searchrequets = db.ServiceRequestLines.Where(x => x.ServiceProvider_ID == serviceProvider.ServiceProvider_ID && x.ServiceProvider_ID != 3 && x.Service.Name == name).ToList();
                if (searchrequets == null || searchrequets.Count() == 0)
                {
                    ViewBag.Status = "Remove";
                    var listofservices = db.ServiceProviderServices.Where(x => x.ServiceProvider_ID == serviceProvider.ServiceProvider_ID);
                    return View(listofservices);
                }
                else
                {
                    ViewBag.Status = "Dont Remove";
                    return View();
                }
            }
            return View();           
        }


        public async Task<ActionResult> Delete(int spid,int servid)
        {
            ServiceProviderService serviceProviderService =  db.ServiceProviderServices.Where(x => x.ServiceProvider_ID == spid && x.ServiceID == servid).FirstOrDefault();
            db.ServiceProviderServices.Remove(serviceProviderService);
            await db.SaveChangesAsync();
            return RedirectToAction("Index","Messages");
        }

    }
}
