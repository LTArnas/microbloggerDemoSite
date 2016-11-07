using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Models.Auth;

namespace microbloggerDemoSite.Controllers
{
    // Note, AllowAnonymous at this level negates any Authorize, etc within the class.
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        // TODO: learn more about the signinmanager.
        // typeof(identitymanager.id) instead of defining string type?
        //private SignInManager<IdentityUser, string> _signInManager;


        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            // Check model
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data detected.");
                return View(model);
            }

            if (model.Password != model.PasswordRepeat)
            {
                ModelState.AddModelError("Password", "Password fields did not match.");
                return View(model);
            }

            // Check if user already exists.
            IdentityUser existingUser = UserManager.FindByName(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "User already exists.");
                return View(model);
            }

            // All clear, create user.
            IdentityUser user = new IdentityUser
            {
                UserName = model.UserName,
                Nick = model.Nick,
                LastPost = DateTime.MinValue,

                ApplicationClaims = new List<ApplicationClaim> {
                    new ApplicationClaim("Nick", model.Nick),
                }
            };

            IdentityResult createResult = UserManager.Create(user, model.Password);
            if (!createResult.Succeeded)
            {
                /*
                foreach (string error in createResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                */
                ModelState.AddModelError("", "Sorry, failed to create account. Try again.");
                return View(model);
            }


            // Authorize the user
            ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            if (identity == null)
                return RedirectLocal(Url.Action("LogIn"));

            AuthenticationManager.SignIn(identity);

            // Finished
            return RedirectLocal(Url.Action("Index", "Home"));
        }

        // GET: Auth
        public ActionResult Index()
        {
            return RedirectToAction("LogIn", new { returnUrl = "/" });
        }

        public ActionResult LogIn(string returnUrl)
        {
            LogInModel viewModel = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LogInModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data detected.");
                return View(model);
            }

            IdentityUser user = UserManager.FindByName(model.Email);
            if (user == null)
            {
                // Failed to find user.
                ModelState.AddModelError("", "Incorrect username or password.");
                return View(model);
            }

            bool correctPassword = UserManager.CheckPassword(user, model.Password);
            if (!correctPassword)
            {
                ModelState.AddModelError("", "Incorrect username or password.");
                return View(model);
            }

            ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            if (identity == null)
            {
                ModelState.AddModelError("", "Sorry, failed to log you in. Try again.");
                return View(model);
            }

            AuthenticationManager.SignIn(identity);

            return RedirectLocal(model.ReturnUrl);
        }

        public ActionResult LogOut()
        {
            return View();
        }

        // TODO: Do we want AntiForgeryToken?
        [HttpPost]
        [ActionName("LogOut")]
        public ActionResult LogOut_Confirm()
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut(User.Identity.AuthenticationType);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UpdatePassword(UpdatePasswordViewModel viewModel)
        {
            if (viewModel == null)
                viewModel = new UpdatePasswordViewModel();

            if (viewModel.Password != viewModel.PasswordRepeat)
                ModelState.AddModelError("Password", "The two new password fields did not match.");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UpdatePassword")]
        public ActionResult UpdatePassword_Confirm(UpdatePasswordViewModel viewModel)
        {
            if (viewModel.Password != viewModel.PasswordRepeat)
            {
                ModelState.AddModelError("Password", "The two new password fields did not match.");
            }

            if (ModelState.IsValid &&
                User.Identity.IsAuthenticated) // To be safe.
            {
                UserManager.ChangePassword(User.Identity.GetUserId(), viewModel.CurrentPassword, viewModel.Password);

                return LogOut_Confirm();
            }

            return View(viewModel);
        }
    }
}