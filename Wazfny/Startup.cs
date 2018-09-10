using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Wazfny.Models;

[assembly: OwinStartupAttribute(typeof(Wazfny.Startup))]
namespace Wazfny
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            CreateDefaultRolesAndUsers();
            ConfigureAuth(app);

        }
        public void CreateDefaultRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole("Admins");
            if (!roleManager.RoleExists("Admins"))
            {
                roleManager.Create(role);
            }
            ApplicationUser user = new ApplicationUser();
            user.UserName = "Hassan_Mohamed";
            user.Email = "hassanmohamed_hm@hotmail.com";
            var check = userManager.Create(user, "Dodohm1234 ");
            if (check.Succeeded)
            {
                userManager.AddToRole(user.Id, "Admins");
            }
        }
    }
}
