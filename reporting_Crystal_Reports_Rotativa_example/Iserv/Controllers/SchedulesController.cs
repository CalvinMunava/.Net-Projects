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
using System.Text;

namespace Iserv.Controllers
{
    public class SchedulesController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: Schedules
        public async Task<ActionResult> Index(DateTime? Date)
        {
            if(Date != null)
            {
                var SearchedApp = db.Schedules.Where(x => x.Start == Date).ToList();
                return View(SearchedApp);
            }   
            else
            {
                return View(await db.Schedules.ToListAsync());
            }


        }

        // GET: Schedules/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = await db.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "schedule_Description,themecolor,scheduleStatus,Subject,Start,End")] Schedule schedule, DateTime Date)
        {
            //further processing
            string cutDate = Date.ToShortDateString();
            string cutStart = DateTime.Parse(schedule.Start.ToString()).ToShortTimeString();
            string cutEnd = DateTime.Parse(schedule.End.ToString()).ToShortTimeString();

            string FixedStart = cutDate + " " + cutStart;
            string FixedEnd = cutDate + " " + cutEnd;

            DateTime Start = DateTime.Parse(FixedStart);
            DateTime End = DateTime.Parse(FixedEnd);

            schedule.Start =  Start;
            schedule.End = End;

            schedule.isFullDay = false;
            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                await db.SaveChangesAsync();
                TempData["Status"] = "Complete";
                return RedirectToAction("Index" , "Service_Provider_Application");
            }
          
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = await db.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "schedule_ID,schedule_Description,themecolor,isFullDay,scheduleStatus,Subject,Start,End")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = await db.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Schedule schedule = await db.Schedules.FindAsync(id);
            db.Schedules.Remove(schedule);
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
