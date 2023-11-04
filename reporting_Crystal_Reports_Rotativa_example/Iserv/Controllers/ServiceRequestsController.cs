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
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Services;
using System.Web.Script.Serialization;

namespace Iserv.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: ServiceRequests
        public async Task<ActionResult> Index()
        {
            var serviceRequests = db.ServiceRequests.Include(s => s.Address).Include(s => s.Consumer).Include(s => s.Service);
            return View(await serviceRequests.ToListAsync());
        }
        public async Task<ActionResult> ServiceRequestsProvider()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //find service provider using username 
            ServiceProvider serviceProvider =  db.ServiceProviders.Where(x => x.Name == user.Name).FirstOrDefault();

            var serviceRequests = db.ServiceRequestLines.Where(x => x.ServiceProvider_ID == serviceProvider.ServiceProvider_ID);

            return View(await serviceRequests.ToListAsync());
        }
        public async Task<ActionResult> IndexConsumer()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //find service provider using username 
            Consumer con = db.Consumers.Where(x => x.Name == user.Name).FirstOrDefault();

            var serviceRequests = db.ServiceRequestLines.Include(s => s.ServiceRequest).Include(s => s.Service).Include(s => s.ServiceProvider).Where(x => x.ServiceRequest.ConsumerID == con.ConsumerID) ;

            return View(await serviceRequests.ToListAsync());
        }
        // GET: ServiceRequests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //find service provider using username 
            Consumer con = db.Consumers.Where(x => x.Name == user.Name).FirstOrDefault();

            var serviceRequests = db.ServiceRequestLines.Include(s => s.ServiceRequest).Include(s => s.Service).Include(s => s.ServiceProvider).Where(x => x.ServiceRequest.ConsumerID == con.ConsumerID && x.ServiceRequestID == id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (serviceRequests == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequests.ToList());
        }
        // GET: ServiceRequests/Create
        public ActionResult Create()
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var founduser = UserManager.GetEmail(user.GetUserId());
            Consumer foundconsumer = db.Consumers.Where(x => x.Email == founduser).FirstOrDefault();
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            ViewBag.TimeID = new SelectList(db.Times, "TimeID", "StartTime");
            ViewBag.ID = foundconsumer.ConsumerID;
            return View();
        }
        [HttpPost]
        [WebMethod]
        public JsonResult GetSecondData(int firstId)
        {

            List<Service> services = db.Services.Where(x => x.CategoryID == firstId).ToList();

            var result = new SelectList(services, "ServiceID", "Name"); //populate result   
            ViewBag.ServiceID = new SelectList(services, "ServiceID", "Name");
            return new JsonResult { Data = result };
        }
        [HttpGet]
        [WebMethod]
        public JsonResult GetThirdData(int firstId)
        {
            
            var gotproviders = (from sps in db.ServiceProviderServices.Include(s => s.Service)
                                where sps.ServiceID == firstId
                                select new
                                {
                                    ServiceProviderName = sps.ServiceProvider.Name,
                                }).ToList();
            db.Configuration.ProxyCreationEnabled = false;

            return new JsonResult { Data = gotproviders, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        
        public async Task<JsonResult> CreateRequest([Bind(Include = "CategoryID,ServiceID,TimeID,Service_Description,Date")] ServiceRequest serviceRequest , HttpPostedFileBase image, string ServCode, string Servicereqid)
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var founduser = UserManager.GetEmail(user.GetUserId());
            var Status = 0;
           
            if (ServCode != "")
            {
                int Servcod = Convert.ToInt32(ServCode);
                if (Servicereqid != "")
                {
                    int Servccheck = Convert.ToInt32(Servicereqid);
                    var serviceRequestcheck = db.ServiceRequestLines.Where(x => x.ServiceRequestID == Servccheck && x.ServiceProvider_ID == Servcod).FirstOrDefault();
                    if (serviceRequestcheck == null)
                    {
                        var foundconsumer = db.Consumers.Where(x => x.Email == founduser).FirstOrDefault();
                        //crate a line object 
                        ServiceRequestLine newLine = new ServiceRequestLine();
                        newLine.ServiceID = serviceRequest.ServiceID;
                        newLine.ServiceRequestID = Servccheck;
                        newLine.ServiceProvider_ID = Servcod;
                        db.ServiceRequestLines.Add(newLine);
                        await db.SaveChangesAsync();
                        Status = Servccheck;
                    }
                }
                else
                {
                    if (image == null)
                    {
                        string Imagepath = "~/Content/ServiceRequests/sricon.png";
                        //fix imagepath
                        serviceRequest.ImagePath = Imagepath;
                    }
                    else
                    {
                        //get and store image
                        string Imagepath = "~/Content/ServiceRequests/" + image.FileName;
                        image.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/ServiceRequests/"), image.FileName));
                        serviceRequest.ImagePath = Imagepath;
                    }
                    //get Logged in Users Address - find cionsumer using username in aspnet table then get the id for consumer and address
                    var foundconsumer = db.Consumers.Where(x => x.Email == founduser).FirstOrDefault();
                    serviceRequest.Address_ID = foundconsumer.AddressID;
                    //get Logged In users Consumer ID 
                    serviceRequest.ConsumerID = foundconsumer.ConsumerID;
                    //change the status ID to sent from consumer 
                    serviceRequest.ServiceRequest_Status_ID = 1;
                    serviceRequest.datecreated = DateTime.Now;
                    db.ServiceRequests.Add(serviceRequest);
                    await db.SaveChangesAsync();


                    //find the request 
                    var serviceRequestfound = db.ServiceRequests.Where(x => x.ConsumerID == foundconsumer.ConsumerID && x.ServiceID == serviceRequest.ServiceID && x.Service_Description == serviceRequest.Service_Description).FirstOrDefault();
                    Status = Convert.ToInt32(serviceRequestfound.ServiceRequestID);

                    //crate a line object 
                    ServiceRequestLine newLine = new ServiceRequestLine();
                    newLine.ServiceID = serviceRequestfound.ServiceID;
                    newLine.ServiceRequestID = serviceRequestfound.ServiceRequestID;
                    newLine.ServiceProvider_ID = Servcod;
                    db.ServiceRequestLines.Add(newLine);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                var findallproviders = db.ServiceProviderServices.Where(x => x.ServiceID == serviceRequest.ServiceID).ToList();
                // create the request
                if (image == null)
                {
                    string Imagepath = "~/Content/ServiceRequests/sricon.png";
                    //fix imagepath
                    serviceRequest.ImagePath = Imagepath;
                }
                else
                {
                    //get and store image
                    string Imagepath = "~/Content/ServiceRequests/" + image.FileName;
                    image.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/ServiceRequests/"), image.FileName));
                    serviceRequest.ImagePath = Imagepath;
                }
                //get Logged in Users Address - find cionsumer using username in aspnet table then get the id for consumer and address
                var foundconsumer = db.Consumers.Where(x => x.Email == founduser).FirstOrDefault();
                serviceRequest.Address_ID = foundconsumer.AddressID;
                //get Logged In users Consumer ID 
                serviceRequest.ConsumerID = foundconsumer.ConsumerID;
                //change the status ID to sent from consumer 
                serviceRequest.ServiceRequest_Status_ID = 1;
                serviceRequest.datecreated = DateTime.Now;
                db.ServiceRequests.Add(serviceRequest);
                await db.SaveChangesAsync();

                
                //find the request 
                var serviceRequestfound = db.ServiceRequests.Where(x => x.ConsumerID == foundconsumer.ConsumerID && x.ServiceID == serviceRequest.ServiceID && x.Service_Description == serviceRequest.Service_Description).FirstOrDefault();                
                //foreach of them create a line record
                foreach(var provider in findallproviders)
                {
                    ServiceRequestLine newLine = new ServiceRequestLine();
                    newLine.ServiceID = serviceRequestfound.ServiceID;
                    newLine.ServiceRequestID = serviceRequestfound.ServiceRequestID;
                    newLine.ServiceProvider_ID = provider.ServiceProvider_ID;
                    db.ServiceRequestLines.Add(newLine);
                    await db.SaveChangesAsync();
                }
                Status = 2121;
            }

            return new JsonResult { Data = Status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult AddProviders(int id, int srid)
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            ViewBag.ID = id;
            ViewBag.SrID = srid;
            return View();
        }

        [HttpGet]
        public JsonResult GetProviders()
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
            Consumer userI = db.Consumers.Where(userr => userr.UserID == userInUser.UserID).FirstOrDefault();

            var fiveAhead = DateTime.Now.AddMinutes(3);
            var fivebefore = DateTime.Now.AddMinutes(-3);
            {
                var gotproviders = (from sps in db.ServiceRequestLines.Include(s => s.ServiceProvider).Include(s => s.ServiceRequest)
                                    where
                                    sps.ServiceRequest.ServiceRequest_Status_ID == 1 &&
                                    sps.ServiceRequest.ConsumerID == userI.ConsumerID &&
                                    sps.ServiceRequest.datecreated <= fiveAhead && sps.ServiceRequest.datecreated >= fivebefore
                                    select new 
                                   {
                                       ServiceProviderName = sps.ServiceProvider.Name,
                                       ServiceProviderBusiness = sps.ServiceProvider.BusinessName,
                                   }).ToList();
                return new JsonResult { Data = gotproviders, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public async Task<ActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            serviceRequest.ServiceRequest_Status_ID = 4;

            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            db.Entry(serviceRequest).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("IndexConsumer");
        }

            // GET: ServiceRequests/Edit/5
            public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.Address_ID = new SelectList(db.Addresses, "AddressID", "StreetName", serviceRequest.Address_ID);
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ConsumerID", "Name", serviceRequest.ConsumerID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", serviceRequest.ServiceID);
            ViewBag.ServiceRequest_Status_ID = new SelectList(db.ServiceRequest_Status, "ServiceRequest_Status_ID", "Status", serviceRequest.ServiceRequest_Status_ID);
            return View(serviceRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceRequestID,Service_Description,Date,Address_ID,ServiceRequest_Status_ID,ConsumerID,ServiceID")] ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Address_ID = new SelectList(db.Addresses, "AddressID", "StreetName", serviceRequest.Address_ID);
            ViewBag.ConsumerID = new SelectList(db.Consumers, "ConsumerID", "Name", serviceRequest.ConsumerID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", serviceRequest.ServiceID);
            ViewBag.ServiceRequest_Status_ID = new SelectList(db.ServiceRequest_Status, "ServiceRequest_Status_ID", "Status", serviceRequest.ServiceRequest_Status_ID);
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return HttpNotFound();
            }
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceRequest serviceRequest = await db.ServiceRequests.FindAsync(id);
            db.ServiceRequests.Remove(serviceRequest);
            await db.SaveChangesAsync();
            return RedirectToAction("IndexConsumer");
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
