using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Iserv.Models;
using System.Web.Services;
using System.Data.Entity;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Identity.Owin;

namespace Iserv.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        ApplicationDbContext context;

        public HomeController()
        {
            context = new ApplicationDbContext();
        }
        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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

        private InternetServicesEntities db = new InternetServicesEntities();

        public ActionResult Landing()
        {
            return View();
        }

        public ActionResult Home()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                var u = UserManager.GetEmail(user.GetUserId());
  
                if (isAdminUser())
                {
                    return RedirectToAction("HomeAdmin");
                }
                if (isServiceProviderUser())
                {
                    return RedirectToAction("HomeProvider");
                }
                if (isCustomerUser())
                {
                    return RedirectToAction("HomeCustomer");
                }
            }
            else
            {
                return RedirectToAction("Landing");
            }
            return RedirectToAction("Landing");
        }

        //Check Role 
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (User.IsInRole("Admin"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public Boolean isCustomerUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (User.IsInRole("Customer"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public Boolean isServiceProviderUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (User.IsInRole("ServiceProvider"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }



        //------------------Home Admin----------------------------------------\\
        public ActionResult HomeAdmin()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            ViewBag.UserName = userInAsp.UserName;
            ViewBag.Time = DateTime.Now.ToShortTimeString();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();
           // userInUser.Status = "online";
           // db.Entry(userInUser).State = EntityState.Modified;
           // db.SaveChangesAsync();

            //change when fixing final
            var Patients = db.Consumers.Count();
            ViewBag.Patients = Patients;
            var Employees = db.ServiceProviders.Count();
            ViewBag.Employees = Employees;
            var SystemUsers = db.AspNetUsers.Count();
            ViewBag.SystemUsers = SystemUsers;
            var Suppliers = db.Services.Count();
            ViewBag.Suppleirs = Suppliers;
            var Orders = db.ServiceOrders.Count();
            ViewBag.Orders = Orders;
            var Procedures = db.Categories.Count();
            ViewBag.Procedures = Procedures;
            var Ailments = db.Cities.Count();
            ViewBag.Ailments = Ailments;
            var Institutes = db.ServiceQuotes.Count();
            ViewBag.Institutes = Institutes;
            var Medical = db.Service_Provider_Application.Count();
            ViewBag.Medical = Medical;
            var Medication = db.ServiceRequests.Count();
            ViewBag.Medication = Medication;

            return View();
        }

        //-------------------Home Customer--------------------------------------\\

        public ActionResult HomeCustomer()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());

            var customer = db.Consumers.Include(s => s.Address).Include(s => s.User).ToList();


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();
            ViewBag.UserName = userInAsp.UserName;
            ViewBag.ImagePath = userInUser.ImagePath;
            ViewBag.Time = DateTime.Now.ToShortTimeString();
            userInUser.Status = "online";
            db.Entry(userInUser).State = EntityState.Modified;
            db.SaveChangesAsync();

            return View(customer);
        }
       //----------------------Home Provider---------------------------------------\\
        public ActionResult HomeProvider()
        {
            ViewBag.Time = DateTime.Now.ToShortTimeString();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var u = UserManager.GetEmail(user.GetUserId());


                //fnd user in ser table
                AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

                //fnd user in ser table
                User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();
                if (userInUser.Status == "NewSP")
                {
                    return RedirectToAction("HomeProviderNew");
                }
                else
                {
                    userInUser.Status = "online";
                    db.Entry(userInUser).State = EntityState.Modified;
                    db.SaveChangesAsync();
                    return View(userInUser);
                }
                 
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetServices()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            var providerid = db.ServiceProviders.Where(x => x.UserID == userInUser.UserID).FirstOrDefault();

            {
                var gotservices = (from sps in db.ServiceProviderServices
                                    where sps.ServiceProvider_ID == providerid.ServiceProvider_ID
                                    select new
                                    {
                                        CategoryName =  sps.Category.Name,
                                        ServiceName = sps.Service.Name,
                                    }).ToList();
                return new JsonResult { Data = gotservices, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult HomeProviderNew()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            var providerid = db.ServiceProviders.Where(x => x.UserID == userInUser.UserID).FirstOrDefault();

            ViewBag.ID = providerid.ServiceProvider_ID;

            /////////////////////////////////////////
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            return View();

        }

        [HttpPost]
        public JsonResult GetSecondData(int firstId)
            {

                List<Service> services = db.Services.Where(x => x.CategoryID == firstId).ToList();

                var result = new SelectList(services, "ServiceID", "Name"); //populate result   
                ViewBag.ServiceID = new SelectList(services, "ServiceID", "Name");
                return new JsonResult { Data = result };
            }

        [HttpPost]
        public async Task<JsonResult> AddServiceProviderServicesAsync(int spid, int servid, int catid)
        {
            //check if these 3 already exist
            var Status = "Nothing";
            //first find the record using linq
            var FoundRecord = db.ServiceProviderServices.Where(x => x.ServiceProvider_ID == spid && x.ServiceID == servid && x.CategoryID == catid).FirstOrDefault();

            if(FoundRecord != null)
            {
                Status = "DuplicateFound";
            }
            else
            {
                ServiceProviderService newService = new ServiceProviderService();
                newService.ServiceProvider_ID = spid;
                newService.ServiceID = servid;
                newService.CategoryID = catid;
                db.ServiceProviderServices.Add(newService);
                db.Configuration.ProxyCreationEnabled = false;
                await db.SaveChangesAsync();
                Status = "AddedNew";
            }
            return new JsonResult { Data = Status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult HomeProviderchange()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var u = UserManager.GetEmail(user.GetUserId());


            //fnd user in ser table
            AspNetUser userInAsp = db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

            //fnd user in ser table
            User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

            userInUser.Status = "online";
            db.Entry(userInUser).State = EntityState.Modified;
            db.SaveChangesAsync();

            return RedirectToAction("HomeProvider");

        }

        //---------------------------Calander Things-------------------------------\\



        public JsonResult GetSchedules()
        {
            var user = User.Identity;
            //find in service provider table
            ServiceProvider serviceProvider = db.ServiceProviders.Where(x => x.Name == user.Name).FirstOrDefault();
            {
                var appointments = (from sd in db.Schedules                                                                  
                                    where sd.scheduleStatus == "Pending"
                                    select new
                                    {
                                        ScheduleID = sd.schedule_ID,
                                        Subject = sd.Subject,
                                        Description = sd.schedule_Description,
                                        Start = sd.Start,
                                        End = sd.End,
                                        Color = sd.themecolor,
                                        Allday = sd.isFullDay,
                                        Meeting = sd.Meetinglink,
                                        ApplicantName = sd.ApplicantName
                                    }).ToList();
                return new JsonResult { Data = appointments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [HttpPost]
        public async Task<JsonResult> AddSchedule(Schedule appointment)
        {

            var status = false;

                if (appointment.schedule_ID > 0)
                {
                    //Update the event
                    var v = db.Schedules.Where(a => a.schedule_ID == appointment.schedule_ID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = appointment.Subject;
                        v.Start = appointment.Start;
                        v.End = appointment.End;
                        v.schedule_Description = appointment.schedule_Description;
                        v.Meetinglink = appointment.Meetinglink;
                        v.isFullDay = false;
                        v.themecolor = "dodgerblue";
                        v.scheduleStatus = "Pending";
                        v.ApplicantName = appointment.ApplicantName;
                        db.Entry(v).State = EntityState.Modified;
                        status = true;
                    }
                }
                else
                {
                     int con = Convert.ToInt32(appointment.ApplicantName);
                     var Name = db.Service_Provider_Application.Where(x => x.SPA_ID == con).First().Name.ToString();
                     appointment.ApplicantName = Name;
                     appointment.themecolor = "dodgerblue";
                     db.Schedules.Add(appointment);
                     status = true;
                }

            db.SaveChangesAsync();
            //send email with details 
            Service_Provider_Application spa = db.Service_Provider_Application.Where(x => x.Name == appointment.ApplicantName).FirstOrDefault();

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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/CalanderApplication.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", spa.Name);
            body = body.Replace("{Message}", "Hi Your Iserv Service Provider Meeting has been booked for" + appointment.Start  + " " + "This is a System Generated Email to Confirm This Booking...");
            await UserManager.SendEmailAsync(user.Id, "Application", body);


            //Remove Application User
            var appuser = db.AspNetUsers.Where(x => x.UserName == spa.Name).FirstOrDefault();
            db.AspNetUsers.Remove(appuser);
            await db.SaveChangesAsync();
            
            return new JsonResult { Data = new { status = status } };
        }


        [HttpPost]
        public async Task<JsonResult> DeleteSchedule(int scheduleID)
        {
            var status = false;
   
                var appointment = db.Schedules.Where(a => a.schedule_ID == scheduleID).FirstOrDefault();

            //send email with details 

            Service_Provider_Application spa = db.Service_Provider_Application.Where(x => x.Name == appointment.ApplicantName).FirstOrDefault();

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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/CalanderApplication.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", spa.Name);
            body = body.Replace("{Message}", "Hi Your Iserv Service Provider Meeting has been booked for" + appointment.Start + " " + "This is a System Generated Email to Confirm This Booking...");
            await UserManager.SendEmailAsync(user.Id, "Application", body);


            //Remove Application User
            var appuser = db.AspNetUsers.Where(x => x.UserName == spa.Name).FirstOrDefault();
            db.AspNetUsers.Remove(appuser);
            await db.SaveChangesAsync();

            if (appointment != null)
                {
                    db.Schedules.Remove(appointment);
                    db.SaveChangesAsync();
                    status = true;
                }


            return new JsonResult { Data = new { status } };
        }

        //----------------------------------------------------------------------------\\

        public ActionResult Print()
        {
            return View();
        }

        //Static Services
        public ActionResult Plumbing()
        {
            return View();
        }

        public ActionResult Electrical()
        {
            return View();
        }

        public ActionResult Garden()
        {
            return View();
        }

        public ActionResult Painting()
        {
            return View();
        }
        public ActionResult Cleaning()
        {
            return View();
        }

     

        public ActionResult General()
        {
            return View();
        }


     //--------------------------------Landing---------------------------------------------\\
    }
}