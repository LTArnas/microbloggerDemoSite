using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Models.User;

namespace microbloggerDemoSite.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Get(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return RedirectLocal("/");
            
            IdentityUser userProfile = UserManager.FindById(userId);

            if (userProfile == null)
                return RedirectLocal("/");

            GetUserViewModel viewModel = new GetUserViewModel
            {
                Id = userProfile.Id,
                LastPost = userProfile.LastPost,
                Nick = userProfile.Nick,
                RecentPosts = userProfile.RecentPosts,
                UserName = userProfile.UserName
            };

            return View(viewModel);
        }

        public ActionResult Update(UpdateUserViewModel viewModel)
        {
            if (viewModel == null)
                viewModel = new UpdateUserViewModel();

            // Double checks.
            if (User.Identity.IsAuthenticated)
            {
                // TODO: Could a loged in (authenticated) user,
                // manually change claim value so GetUserId() returns some other user's ID?
                // ...if so, all the user info this returns is not protected.
                // Potential solution, if needed, is to have a challange re-login page, before this page, maybe?
                IdentityUser userProfile = UserManager.FindById(User.Identity.GetUserId());
                if (userProfile == null)
                    return RedirectLocal("/");

                viewModel.Nick = userProfile.Nick;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public ActionResult Update_Confirm(UpdateUserViewModel viewModel)
        {
            if (ModelState.IsValid && 
                User.Identity.IsAuthenticated) // To be safe.
            {
                // TODO: We are doing a round-trip to update. Would be nice to just update in one call.
                IdentityUser userProfile = UserManager.FindById(User.Identity.GetUserId());
                if (userProfile == null)
                    return RedirectLocal("/");

                userProfile.Nick = viewModel.Nick;

                IdentityResult updateResult = UserManager.Update(userProfile);
                ViewBag.Success = updateResult.Succeeded;
            }

            return View(viewModel);
        }
    }
}