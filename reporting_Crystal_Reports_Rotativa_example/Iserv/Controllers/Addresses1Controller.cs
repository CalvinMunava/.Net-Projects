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
    public class Addresses1Controller : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();

        // GET: Addresses1
        public async Task<ActionResult> Index()
        {
            var addresses = db.Addresses.Include(a => a.City).Include(a => a.Province).Include(a => a.Address1).Include(a => a.Address2);
            return View(await addresses.ToListAsync());
        }

        // GET: Addresses1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // GET: Addresses1/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name");
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "Name");
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName");
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName");
            return View();
        }

        // POST: Addresses1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AddressID,StreetNumber,StreetName,CityID,ProvinceID,UserID")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", address.CityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "Name", address.ProvinceID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", address.AddressID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", address.AddressID);
            return View(address);
        }

        // GET: Addresses1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", address.CityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "Name", address.ProvinceID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", address.AddressID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", address.AddressID);
            return View(address);
        }

        // POST: Addresses1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AddressID,StreetNumber,StreetName,CityID,ProvinceID,UserID")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "Name", address.CityID);
            ViewBag.ProvinceID = new SelectList(db.Provinces, "ProvinceID", "Name", address.ProvinceID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", address.AddressID);
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", address.AddressID);
            return View(address);
        }

        // GET: Addresses1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Addresses1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            db.Addresses.Remove(address);
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
