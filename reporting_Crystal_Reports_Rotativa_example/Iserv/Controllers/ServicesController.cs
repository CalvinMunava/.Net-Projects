using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Iserv.Models;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iserv.Controllers
{
    public class ServicesController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: Services
        public async Task<ActionResult> Index( string searchBy, string search)
        {
          if (searchBy == "Name")
          {
            var services = db.Services.Include(c => c.Category);
           return View(await services.Where(x => x.Name.StartsWith(search) || search == null).ToListAsync());
          }
          else
          {
            var services = db.Services.Include(c => c.Category);
            return View(await services.Where(x => x.Name.StartsWith(search) || search == null).ToListAsync());
          }
    }





        // GET: Services/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServiceID,Name,Description,ImagePath,CategoryID")] Service service, HttpPostedFileBase Blob)
        {
            if (ModelState.IsValid)
            {
                //get and store image
                /*string Imagepath = "~/Content/Services/" + Blob.FileName;
                Blob.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/Services/"), Blob.FileName));
                
                // fix imagepath
                service.ImagePath = "~/Content/Services/" + Blob.FileName;*/

                db.Services.Add(service);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", service.CategoryID);
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", service.CategoryID);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceID,Name,Description,ImagePath,CategoryID")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", service.CategoryID);
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Service service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
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



        //---------------------------------------------------------Used This------------------------------------------------------------\\
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
          
            var services = (from serv in db.Services
                              where serv.Name.StartsWith(prefix)
                              select new
                              {
                                  label = serv.Name,
                                  val = serv.ServiceID,
                                  cat = serv.CategoryID,
                              }).ToList();

            return Json(services);
        }


       
        [HttpPost]
        public async Task<JsonResult> AddServiceProviderServicesAsync( int id, int serviceid, int categoryid)
        {

                ServiceProviderService newService = new ServiceProviderService();
                newService.ServiceProvider_ID = id;
                newService.ServiceID = serviceid;
                newService.CategoryID = categoryid;
                db.ServiceProviderServices.Add(newService);
                await db.SaveChangesAsync();

                db.Configuration.ProxyCreationEnabled = false;
                List<ServiceProviderService>  Added = await db.ServiceProviderServices.Where(p => p.ServiceProvider_ID == id).ToListAsync();
                       
                return new JsonResult { Data = Added, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


    }
}
