using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using microbloggerDemoSite.Identity;

namespace microbloggerDemoSite.Controllers
{
    public class SearchController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectLocal(Url.Action("Nick", "Search"));
        }

        [AllowAnonymous]
        public ActionResult Nick(string query = "")
        {
            List<IdentityUser> result = UserManager.Users
                .Where(u => u.Nick.Contains(query))
                //.Skip(0) // Could be used as a quick and dirty way to do 'more'/'next page'.
                .Take(100)
                .OrderBy(u => u.Nick)
                .ToList();
            
            return View(result);
        }
    }
}