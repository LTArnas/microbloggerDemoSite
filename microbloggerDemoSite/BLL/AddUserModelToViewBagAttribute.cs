using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using microbloggerDemoSite.Identity;

namespace microbloggerDemoSite.BLL
{
    public class AddUserModelToViewBagAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            IOwinContext owinContext = filterContext.HttpContext.GetOwinContext();
            UserManager<IdentityUser> userManager = owinContext.GetUserManager<UserManager<IdentityUser>>();
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                IdentityUser user = userManager.FindByName(filterContext.HttpContext.User.Identity.Name);
                if (user != null)
                    filterContext.Controller.ViewBag.User = user;
            }
        }
    }
}
