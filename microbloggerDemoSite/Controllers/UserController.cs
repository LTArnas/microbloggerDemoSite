using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Controllers;

namespace microbloggerDemoSite.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read(string userId)
        {
            IdentityUser userProfile;

            if (string.IsNullOrWhiteSpace(userId))
                return RedirectLocal("/");
            else
                userProfile = UserManager.FindById(userId);

            if (userProfile == null)
                return RedirectLocal("/");
            else
                return View(userProfile);
        }
    }
}