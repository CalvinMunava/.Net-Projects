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

namespace Iserv.Controllers
{
    public class ConsumersController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: Consumers
        public async Task<ActionResult> Index(string searchBy, string search)
        {
            if (searchBy == "Description")
            {
                var consumers = db.Consumers.Include(c => c.User).Include(c => c.Address);
                return View(await consumers.Where(x=> x.Email == search || search == null).ToListAsync());
            }
            else
            {
                var consumers = db.Consumers.Include(c => c.User).Include(c => c.Address);
                return View(await consumers.Where(x => x.Name.StartsWith(search) || search == null).ToListAsync());
            }
        }

        // GET: Consumers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = await db.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // GET: Consumers/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName");
            return View();
        }

        // POST: Consumers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ConsumerID,Name,Surname,Age,Email,ID_Number,Cell_Number,Gender,UserID,AddressID")] Consumer consumer)
        {
            if (ModelState.IsValid)
            {
                db.Consumers.Add(consumer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", consumer.UserID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", consumer.AddressID);
            return View(consumer);
        }

        // GET: Consumers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = await db.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", consumer.UserID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", consumer.AddressID);
            return View(consumer);
        }

        // POST: Consumers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ConsumerID,Name,Surname,Age,Email,ID_Number,Cell_Number,Gender,UserID,AddressID")] Consumer consumer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", consumer.UserID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", consumer.AddressID);
            return View(consumer);
        }

        // GET: Consumers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumer consumer = await db.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return HttpNotFound();
            }
            return View(consumer);
        }

        // POST: Consumers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Consumer consumer = await db.Consumers.FindAsync(id);
            db.Consumers.Remove(consumer);
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
