using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Blog;
using microbloggerDemoSite.DAL;

namespace microbloggerDemoSite
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // TODO: Consider static create method(s), as per template example.
            app.CreatePerOwinContext(() => { return new MongoDbContext(); });
            // TODO: Consider creating custom UserManager, as per template example.
            app.CreatePerOwinContext<UserManager<IdentityUser>>((options, owin) =>
            {
                // Create
                MongoDbContext dbContext = owin.Get<MongoDbContext>();
                UserStore store = new UserStore(dbContext);
                UserManager<IdentityUser> manager = new UserManager<IdentityUser>(store);
                // Configure
                manager.PasswordHasher = new PasswordHasher();
                // Finished
                return manager;
            });
            app.CreatePerOwinContext<BlogManager>((options, owin) =>
            {
                UserManager<IdentityUser> userManager = owin.Get<UserManager<IdentityUser>>();
                MongoDbContext dbContext = owin.Get<MongoDbContext>();
                BlogManager manager = new BlogManager(userManager, dbContext);
                return manager;
            });
            // TODO: setup signinmanager

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                // This should be used for production version.
                // (Except our production setup doesn't have SSL, so we will never set this.)
                //CookieSecure = CookieSecureOption.Always,
                AuthenticationMode = AuthenticationMode.Active,
                LoginPath = new PathString("/auth/login"),
                ExpireTimeSpan = TimeSpan.FromHours(1),
                SlidingExpiration = true
            });
        }
    }
}
