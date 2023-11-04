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
    public class ProviderAppointmentsController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();
        private ApplicationUserManager _userManager;
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
        // GET: ProviderAppointments
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


            //find consumer 
            ServiceProvider prov = db.ServiceProviders.Where(x => x.UserID == userInUser.UserID).FirstOrDefault();

            var providerAppointments = db.ProviderAppointments.Include(p => p.ServiceProvider).Where(x => x.ServiceProvider_ID == prov.ServiceProvider_ID);
            ViewBag.ServiceProviderID = prov.ServiceProvider_ID;
            ViewBag.Status = TempData["Status"] as string;
            return View(await providerAppointments.ToListAsync());
        }


        public async Task<ActionResult> IndexMake(DateTime? Date)
        {
            if (Date != null)
            {
                var SearchedApp = db.ProviderAppointments.Where(x => x.Start == Date).ToList();
                return View(SearchedApp);
            }
            else
            {
                return View(await db.ProviderAppointments.ToListAsync());
            }
        }






        //-------------------------------------------Added--------------------------------\\
        public JsonResult GetSchedules()
        {
            var user = User.Identity;
            //find in service provider table
            ServiceProvider serviceProvider = db.ServiceProviders.Where(x => x.Name == user.Name).FirstOrDefault();
            {
                var appointments = (from sd in db.ProviderAppointments
                                    where sd.scheduleStatus == "Pending" && sd.ServiceProvider_ID == serviceProvider.ServiceProvider_ID
                                    select new
                                    {
                                        ProviderAppointmentID = sd.ProviderAppointmentID,
                                        Subject = sd.Subject,
                                        Description = sd.schedule_Description,
                                        Start = sd.Start,
                                        End = sd.End,
                                        Color = sd.themecolor,
                                    }).ToList();
                return new JsonResult { Data = appointments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddSchedule(ProviderAppointment appointment)
        {
            var status = false;
            var user = User.Identity;
            //find in service provider table
            ServiceProvider serviceProvider = db.ServiceProviders.Where(x => x.Name == user.Name).FirstOrDefault();

            if (appointment.ProviderAppointmentID > 0)
            {
                //Update the event
                var v = db.ProviderAppointments.Where(a => a.ProviderAppointmentID == appointment.ProviderAppointmentID).FirstOrDefault();
                if (v != null)
                {
                    v.Subject = appointment.Subject;
                    v.Start = appointment.Start;
                    v.End = appointment.End;
                    v.schedule_Description = appointment.schedule_Description;
                    v.ServiceProvider_ID = serviceProvider.ServiceProvider_ID;
                    v.isFullDay = false;
                    v.themecolor = "dodgerblue";
                    v.scheduleStatus = "Pending";
                    db.Entry(v).State = EntityState.Modified;
                    status = true;
                    ViewBag.Status = "Updated";
                }
            }
            else
            {
                appointment.ServiceProvider_ID = serviceProvider.ServiceProvider_ID;
                appointment.themecolor = "dodgerblue";
                appointment.isFullDay = false;
                db.ProviderAppointments.Add(appointment);
                status = true;
            }
            
            await  db.SaveChangesAsync();
            return new JsonResult { Data = new { status = status } };
        }


        [HttpPost]
        public async Task<JsonResult> DeleteSchedule(int ProviderAppointmentID)
        {
            var status = false;

            var v = db.ProviderAppointments.Where(a => a.ProviderAppointmentID == ProviderAppointmentID).FirstOrDefault();
            if (v != null)
            {
                db.ProviderAppointments.Remove(v);
                await db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status } };
        }



    }
}
