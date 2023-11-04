using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Iserv.Models;
using System.IO;
using Microsoft.AspNet.Identity.Owin;

namespace Iserv.Controllers
{
    public class UsersController : Controller
    {

        private InternetServicesEntities db = new InternetServicesEntities();


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        ApplicationDbContext context;

        public UsersController()
        {
            context = new ApplicationDbContext();
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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

        // GET: Users
        public async Task<ActionResult> Index(string searchBy, string date, string name)
        {

            if (searchBy == "Name")
            {
                var users = db.Users.Include(u => u.AspNetUser);
                return View(await users.Where(x=> x.UserName == name ).ToListAsync());
            }
            else if(searchBy == "Date")
            {
                var users = db.Users.Include(u => u.AspNetUser);
                DateTime chkdate = Convert.ToDateTime(date);
                return View(await users.Where(x => x.datejoined == chkdate ).ToListAsync());
            }
            else
            {
                var Today = DateTime.Today;
                DateTime LastWeek = Today.AddDays(-7);
                var users = db.Users.Include(u => u.AspNetUser);
                var list = users.Where(x => x.datejoined > LastWeek && x.Activity == "Enabled").ToList();

                return View(list);
            }

        }


        //checked
        public JsonResult DisableUser(string UserID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var status = "okay";
            //..find user in User Table 
            int ID = Convert.ToInt32(UserID);

            var FoundUser = db.Users.Where(x => x.UserID == ID).FirstOrDefault();
            FoundUser.Activity = "Disabled";

            var FoundSystemUser = db.AspNetUsers.Where(x => x.Id == FoundUser.AspNetID).FirstOrDefault();
            FoundSystemUser.EmailConfirmed = false;
            //change password and alert them

           
            var user =  UserManager.FindById(FoundSystemUser.Id);
            UserManager.RemovePassword(user.Id);


            db.Entry(FoundUser).State = EntityState.Modified;
            db.Entry(FoundSystemUser).State = EntityState.Modified;
            db.SaveChangesAsync();

            status = "success";

            return new JsonResult { Data = new { status } };


        }

        public async Task<JsonResult> EnableUserAsync(string UserID)
        {

            var status = "okay";
            //..find user in User Table 
            int ID = Convert.ToInt32(UserID);
            var FoundUser = db.Users.Where(x => x.UserID == ID).FirstOrDefault();

            var FoundSystemUser = db.AspNetUsers.Where(x => x.Id == FoundUser.AspNetID).FirstOrDefault();
            //change password and alert them

            FoundUser.Activity = "Enabled";

            //generate password reset token
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(FoundSystemUser.Id);

            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = FoundSystemUser.Id, code = code }, protocol: Request.Url.Scheme);

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/AccountEnabled.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{ConfirmationLink}", callbackUrl);
            body = body.Replace("{UserName}", FoundUser.UserName);
            await UserManager.SendEmailAsync(FoundSystemUser.Id, "Re Enable your Account", body);


            db.Entry(FoundUser).State = EntityState.Modified;
            db.Entry(FoundSystemUser).State = EntityState.Modified;
            db.SaveChangesAsync();

            status = "success";

            return new JsonResult { Data = new { status } };


        }
    

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.AspNetID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Users/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserID,UserName,description,service,AspNetID,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetID = new SelectList(db.AspNetUsers, "Id", "Email", user.AspNetID);
            return View(user);
        }

        // GET: Users/Edit/5



        public async Task<ActionResult> Modify(int id)
        {
                //fnd user in ser table
                User userInUser = db.Users.Where(userr => userr.UserID == id).FirstOrDefault();
                return View(userInUser);      
        }


        public async Task<ActionResult> Edit()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var u = UserManager.GetEmail(user.GetUserId());


                //fnd user in ser table
                AspNetUser userInAsp =  db.AspNetUsers.Where(x => x.Email == u).FirstOrDefault();

                //fnd user in ser table
                User userInUser = db.Users.Where(userr => userr.AspNetID == userInAsp.Id).FirstOrDefault();

                return View(userInUser);
            }

            return RedirectToAction("Error", "Error");
        }


        public async Task<ActionResult> EditConsumer()
        {
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

                return View(userInUser);
            }

            return RedirectToAction("Error", "Error");
        }


        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserID,description")] User user , HttpPostedFileBase Blob)
        {

            if (ModelState.IsValid)
            {
                User userInUser = db.Users.Where(userr => userr.UserID == user.UserID).FirstOrDefault();
                if (Blob == null)
                {
                    userInUser.description = user.description;

                    db.Entry(userInUser).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectToAction("HomeProvider", "Home");
                }
                else
                {
                   
                    //get and store image
                    Blob.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/ProfilePhotos/"), Blob.FileName));
                    userInUser.ImagePath = "~/Content/ProfilePhotos/" + Blob.FileName;
                    userInUser.description = user.description;

                    db.Entry(userInUser).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectToAction("HomeProvider", "Home");
                }
   
            }
            ViewBag.AspNetID = new SelectList(db.AspNetUsers, "Id", "Email", user.AspNetID);
            return RedirectToAction("Error","Ërror");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditConsumer([Bind(Include = "UserID,description")] User user, HttpPostedFileBase Blob)
        {

            if (ModelState.IsValid)
            {
                User userInUser = db.Users.Where(userr => userr.UserID == user.UserID).FirstOrDefault();
                if (Blob == null)
                {
                    userInUser.description = user.description;

                    db.Entry(userInUser).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectToAction("HomeCustomer", "Home");
                }
                else
                {

                    //get and store image
                    Blob.SaveAs(Path.Combine(HttpContext.Server.MapPath("~/Content/ProfilePhotos/"), Blob.FileName));
                    userInUser.ImagePath = "~/Content/ProfilePhotos/" + Blob.FileName;
                    userInUser.description = user.description;

                    db.Entry(userInUser).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectToAction("HomeCustomer", "Home");
                }

            }
            ViewBag.AspNetID = new SelectList(db.AspNetUsers, "Id", "Email", user.AspNetID);
            return RedirectToAction("Error", "Ërror");
        }
        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
