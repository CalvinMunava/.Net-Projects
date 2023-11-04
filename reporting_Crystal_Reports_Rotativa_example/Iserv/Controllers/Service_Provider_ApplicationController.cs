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
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Notifications;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web.Services;

namespace Iserv.Controllers
{
    public class Service_Provider_ApplicationController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        ApplicationDbContext context;
        InternetServicesEntities db = new InternetServicesEntities();


        public Service_Provider_ApplicationController()
        {
            context = new ApplicationDbContext();
        }

        public Service_Provider_ApplicationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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

        // GET: Service_Provider_Application
        public async Task<ActionResult> Index()
        {
            var users = db.Service_Provider_Application.Where(x => x.SPA_Status_ID == 1).ToList();
            ViewBag.UserID = new SelectList(users, "SPA_ID", "Name");
            ViewBag.Status = TempData["Status"] as string;
            var service_Provider_Application = db.Service_Provider_Application.Include(s => s.Service_Provider_Application_Status).Where( x => x.SPA_Status_ID == 1 || x.SPA_Status_ID == 4);
            return View(await service_Provider_Application.ToListAsync());
        }

        // GET: Service_Provider_Application/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service_Provider_Application service_Provider_Application = await db.Service_Provider_Application.FindAsync(id);
            if (service_Provider_Application == null)
            {
                return HttpNotFound();
            }
            return View(service_Provider_Application);
        }

        // GET: Service_Provider_Application/Create
        public ActionResult Create()
        {
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "Name");
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");

            return View();
        }

        // POST: Service_Provider_Application/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Surname,Email,GenderID,ProvinceID,CityID,CellNumber,StreetName,StreetNumber,VatNumber,BusinessName,IdNumber")] Service_Provider_Application service_Provider_Application, HttpPostedFileBase image )
        {
            if (ModelState.IsValid)
            {
                //Convert Status
                service_Provider_Application.SPA_Status_ID = 1;            

                //get and store image
                string Imagepath = "~/Content/ServiceApplications/" + image.FileName;
                image.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/ServiceApplications/"), image.FileName));

                service_Provider_Application.ldimage = "~/Content/ServiceApplications/" + image.FileName;
 

                //create password
                service_Provider_Application.Password = "Password2020!";


                db.Service_Provider_Application.Add(service_Provider_Application); 
                await db.SaveChangesAsync();

                return RedirectToAction("ApplicationSent");
            }

            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", service_Provider_Application.ServiceID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", service_Provider_Application.GenderID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "Name", service_Provider_Application.ProvinceID);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", service_Provider_Application.CityID);
            return View(service_Provider_Application);
        }


     

        public ActionResult ApplicationSent()
        {
            return View();
        }


        // GET: Service_Provider_Application/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service_Provider_Application service_Provider_Application = await db.Service_Provider_Application.FindAsync(id);
            if (service_Provider_Application == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", service_Provider_Application.ServiceID);
            ViewBag.SPA_Status_ID = new SelectList(db.Service_Provider_Application_Status, "SPA_Status_ID", "Status", service_Provider_Application.SPA_Status_ID);
            return View(service_Provider_Application);
        }

        // POST: Service_Provider_Application/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SPA_ID,FullName,Email,Description,ldimage,SPA_Status_ID,ServiceID")] Service_Provider_Application service_Provider_Application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service_Provider_Application).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", service_Provider_Application.ServiceID);
            ViewBag.SPA_Status_ID = new SelectList(db.Service_Provider_Application_Status, "SPA_Status_ID", "Status", service_Provider_Application.SPA_Status_ID);
            return View(service_Provider_Application);
        }

        // GET: Service_Provider_Application/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service_Provider_Application service_Provider_Application = await db.Service_Provider_Application.FindAsync(id);
            if (service_Provider_Application == null)
            {
                return HttpNotFound();
            }
            return View(service_Provider_Application);
        }

        // POST: Service_Provider_Application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Service_Provider_Application service_Provider_Application = await db.Service_Provider_Application.FindAsync(id);
            db.Service_Provider_Application.Remove(service_Provider_Application);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [WebMethod]
        public JsonResult GetSecondData(int firstId)
        {

            List<City> cities  = db.Cities.Where(x => x.ProvinceID == firstId).ToList();
            
            var result = new SelectList(cities, "CityID", "Name"); //populate result   
            ViewBag.CityID = new SelectList(cities, "CityID", "Name");
            return new JsonResult { Data = result };
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public async Task<ActionResult> RequestMeetingAsync(int id)
        {
            Service_Provider_Application spa = db.Service_Provider_Application.Where(x => x.SPA_ID == id).FirstOrDefault();


            //CREATE Register User
            RegisterViewModel model = new RegisterViewModel();
            model.UserName = spa.Name;
            model.Email = spa.Email;
            model.Password = "ServiceProvider2020!";
          

            //CREATE APPLICATION User
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = spa.CellNumber };

            var result = await UserManager.CreateAsync(user, model.Password);
            await db.SaveChangesAsync();
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var result1 = UserManager.AddToRole(user.Id, "ServiceProvider");

            await db.SaveChangesAsync();

            spa.SPA_Status_ID = 4;
            db.Entry(spa).State = EntityState.Modified;
            db.SaveChangesAsync();

            
            var callbackUrl = Url.Action("Create", "ScheduleAppoitnments", new { id = spa.SPA_ID });
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/ScheduleAppointment.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{ConfirmationLink}", callbackUrl);
            body = body.Replace("{UserName}", spa.Name);
            await UserManager.SendEmailAsync(user.Id, "Congratulations", body);

            TempData["Status"] = "Meeting Requested";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Reject(int id)
        {
            Service_Provider_Application spa = db.Service_Provider_Application.Where(x => x.SPA_ID == id).FirstOrDefault();
            spa.SPA_Status_ID = 3;
            db.Entry(spa).State = EntityState.Modified;
            await db.SaveChangesAsync();


            //CREATE Register User
            RegisterViewModel model = new RegisterViewModel();
            model.UserName = spa.Name;
            model.Email = spa.Email;
            model.Password = "ServiceProvider2020!";

            //CREATE APPLICATION User
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = spa.CellNumber };
            var result = await UserManager.CreateAsync(user, model.Password);
            await db.SaveChangesAsync();

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/RejectedApplication.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", spa.Name);
            await UserManager.SendEmailAsync(user.Id, "Application", body);


            //Remove Application User
            var appuser = db.AspNetUsers.Where(x => x.UserName == spa.Name).FirstOrDefault();
            db.AspNetUsers.Remove(appuser);
            await db.SaveChangesAsync();
            TempData["Status"] = "Application Rejected";
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> SendReminderAsync(int id)
        {
            Service_Provider_Application spa = db.Service_Provider_Application.Where(x => x.SPA_ID == id).FirstOrDefault();
            //CREATE APPLICATION User
            var user = db.AspNetUsers.Where(x => x.UserName == spa.Name).FirstOrDefault();
   
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/Reminder.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", spa.Name);
            await UserManager.SendEmailAsync(user.Id, "Reminder", body);
            TempData["Status"] = "Reminder Sent";
            return RedirectToAction("Index");
        }



        //Accept and Reject
        public async Task<ActionResult> Accept(int id)
        {
            Service_Provider_Application spa = db.Service_Provider_Application.Where(x => x.SPA_ID == id).FirstOrDefault();
            spa.SPA_Status_ID = 2;
            db.Entry(spa).State = EntityState.Modified;
            await db.SaveChangesAsync();

            var user = db.AspNetUsers.Where(x => x.UserName == spa.Name).FirstOrDefault();

            //create as User
            //ADD THEM AS A USER 
            User newCreatedUser = new User();
            newCreatedUser.UserName = spa.Name;
            newCreatedUser.description = "Availible";
            newCreatedUser.AspNetID = user.Id;
            newCreatedUser.datejoined = DateTime.Today;
            newCreatedUser.Role = "ServiceProvider";
            newCreatedUser.Activity = "Enabled";
            newCreatedUser.Status = "NewSP";
            newCreatedUser.ImagePath = spa.ldimage;
            db.Users.Add(newCreatedUser);
            await db.SaveChangesAsync();

            //first find consumer based offtheir username after creating them 
            var UsertoFind = db.Users.Where(x => x.UserName == spa.Name).FirstOrDefault();

            //Add Address details 
            Address address = new Address();
            address.StreetName = spa.StreetName;
            address.StreetNumber = Convert.ToInt32(spa.StreetNumber);
            address.CityID = spa.CityID;
            address.ProvinceID = spa.ProvinceID;
            address.UserID = UsertoFind.UserID;
            db.Addresses.Add(address);
            await db.SaveChangesAsync();


            //first find consumer based offtheir username after creating them 
            var addresstofind= db.Addresses.Where(x => x.UserID == UsertoFind.UserID).FirstOrDefault();
       


            ServiceProvider sp = new ServiceProvider();
            sp.Name = spa.Name;
            sp.Surname = spa.Surname;
            sp.BusinessName = spa.BusinessName;
            sp.AverageRating = 5;
            sp.ID_Number = spa.IdNumber;
            sp.UserID = UsertoFind.UserID;
            sp.AddressID = addresstofind.AddressID;
            sp.GenderID = spa.GenderID;
            db.ServiceProviders.Add(sp);
            await db.SaveChangesAsync();

            //-------------------Send Email to New Provider--------------------------
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/SpConfirm.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{ConfirmationLink}", callbackUrl);
            body = body.Replace("{UserName}", spa.Name);
            await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);
            TempData["Status"] = "Application Accepted";
            return RedirectToAction("Index");
            
        }




    }
}
