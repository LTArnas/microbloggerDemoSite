using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using microbloggerDemoSite.Identity;

namespace microbloggerDemoSite.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        //[microbloggerDemoSite.BLL.AddUserModelToViewBag]
        public ActionResult Index()
        {
            List<IdentityUser> recentActiveUsers = UserManager.Users.Where(x => x.RecentPosts != null).OrderByDescending(x => x.LastPost).Take(10).ToList();
            return View(recentActiveUsers);
        }
    }
}