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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using Microsoft.AspNet.Identity.Owin;

namespace Iserv.Controllers
{
    public class AdminsController : Controller
    {
        private InternetServicesEntities db = new InternetServicesEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        ApplicationDbContext context;

        public AdminsController()
        {
            context = new ApplicationDbContext();
        }

        public AdminsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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

        // GET: Admins
        public async Task<ActionResult> Index()
        {
            var admins = db.Admins.Include(a => a.Address).Include(a => a.User).Include(a => a.Gender);
            return View(await admins.ToListAsync());
        }

        // GET: Admins/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = await db.Admins.FindAsync(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {


            ViewBag.CityID = new SelectList(db.Cities.Select(c => new { c.CityID, c.Name }), "CityID", "Name");
            ViewBag.ProvinceID = new SelectList(db.Provinces.Select(c => new { c.ProvinceID, c.Name }), "ProvinceID", "Name");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AdminID,Name,Surname,Age,Email,ID_Number,Cell_Number,Gender,UserID,AddressID,GenderID")] Admin admin , [Bind(Include = "StreetNumber,StreetName,CityID,ProvinceID")] Address address)
        {
            RegisterViewModel model = new RegisterViewModel();
            model.UserName = admin.Name;
            model.Email = admin.Email;           
            model.Password = "Iserv1234!";

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = admin.Cell_Number };

            var result = await UserManager.CreateAsync(user, model.Password);

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var result1 = UserManager.AddToRole(user.Id, "Admin");
            await db.SaveChangesAsync();

            if (result.Succeeded && result1.Succeeded)
            {

                //first find ASPuSERbased offtheir username after creating them 
                var ASPtoFind = db.AspNetUsers.Where(x => x.UserName == model.UserName).FirstOrDefault();

                //ADD THEM AS A USER 
                User newCreatedUser = new User();
                newCreatedUser.UserName = model.UserName;
                newCreatedUser.description = "Availible";
                newCreatedUser.AspNetID = ASPtoFind.Id;
                newCreatedUser.datejoined = DateTime.Today;
                newCreatedUser.Role = "Admin";
                newCreatedUser.Activity = "Enabled";
                //save image 

                //get and store image

                newCreatedUser.ImagePath = "~/Content/Pictures/Admin.png";
                db.Users.Add(newCreatedUser);
                await db.SaveChangesAsync();

                //first find admin based offtheir username after creating them 
                var UsertoFind = db.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();
                admin.UserID = Convert.ToInt32(UsertoFind.UserID);
                db.Admins.Add(admin);
                await db.SaveChangesAsync();


                //first find consumer based offtheir username after creating them 
                var admint = db.Admins.Where(x => x.Name == model.UserName).FirstOrDefault();

                //Add Address details 
                address.UserID = admint.UserID;
                db.Addresses.Add(address);
                await db.SaveChangesAsync();


                //first find consumer based offtheir username after creating them in address table
                var AddresstoFind = db.Addresses.Where(x => x.UserID == admint.UserID).FirstOrDefault();


                //UPDATE ADDRESS RECORD AGAIN 
                //first find consumer based offtheir username after creating them 
                var ConsumertoFindaGAIN = db.Admins.Where(x => x.Name == admint.Name).FirstOrDefault();
                ConsumertoFindaGAIN.AddressID = AddresstoFind.AddressID;
                db.Entry(ConsumertoFindaGAIN).State = EntityState.Modified;
                await db.SaveChangesAsync();



                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/AccountConfirmation.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{ConfirmationLink}", callbackUrl);
                body = body.Replace("{UserName}", admint.Name);
                body = body.Replace("{Password}", model.Password);
                await UserManager.SendEmailAsync(user.Id, "Confirm your account", body);
                return RedirectToAction("Index");
            }

            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", admin.AddressID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", admin.UserID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", admin.GenderID);
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = await db.Admins.FindAsync(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", admin.AddressID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", admin.UserID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", admin.GenderID);
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AdminID,Name,Surname,Age,Email,ID_Number,Cell_Number,Gender,UserID,AddressID,GenderID")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "AddressID", "StreetName", admin.AddressID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", admin.UserID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", admin.GenderID);
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = await db.Admins.FindAsync(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Admin admin = await db.Admins.FindAsync(id);
            db.Admins.Remove(admin);
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
