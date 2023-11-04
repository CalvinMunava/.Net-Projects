using Microsoft.Owin;
using Owin;
using Iserv.Models;
using Iserv.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web.Http.Results;

[assembly: OwinStartupAttribute(typeof(Iserv.Startup))]
namespace Iserv
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();

         
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            try
            {
                if (!roleManager.RoleExists("Admin"))
                {

                    var role = new IdentityRole();
                    role.Name = "Admin";
                    roleManager.Create(role);


                    var user = new ApplicationUser();
                    user.UserName = "IservAdmin";
                    user.Email = "Iservsystem@gmail.com";

                    string userPWD = "Iserv1234!";

                    var chkUser = UserManager.Create(user, userPWD);
                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user.Id, "Admin");
                    }
                }
                if (!roleManager.RoleExists("Customer"))
                {
                    var role = new IdentityRole();
                    role.Name = "Customer";
                    roleManager.Create(role);

                }

                if (!roleManager.RoleExists("ServiceProvider"))
                {
                    var role = new IdentityRole();
                    role.Name = "ServiceProvider";
                    roleManager.Create(role);

                }
            }
            catch (Exception)
            {
            }
        }
    }
}
