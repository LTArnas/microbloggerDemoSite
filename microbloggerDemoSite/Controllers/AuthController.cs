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
using microbloggerDemoSite.Models;

namespace microbloggerDemoSite.Controllers
{
    // Note, AllowAnonymous at this level negates any Authorize, etc within the class.
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        // TODO: learn more about the signinmanager.
        // typeof(identitymanager.id) instead of defining string type?
        //private SignInManager<IdentityUser, string> _signInManager;

        // TODO: Delete this if base does not have constructor definition anymore.
        public AuthController() :
            base()
        {
        }
        
        public ActionResult Register()
        {
            return View(new IdentityUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(IdentityUser model)
        {
            // Check model
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data detected.");
                return View(model);
            }

            // Check if user already exists.
            IdentityUser existingUser = UserManager.FindByName(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "User already exists.");
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
                foreach (string error in createResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }


            // Authorize the user
            ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            if (identity == null)
                throw new NullReferenceException();

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
                return View(model);
            }

            bool correctPassword = UserManager.CheckPassword(user, model.Password);
            if (!correctPassword)
                return View(model);

            ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            if (identity == null)
                throw new NullReferenceException();

            // TODO: Setup proper claims as part of the user (implement the necessary interfaces, etc)
            /*
            ClaimsIdentity identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.UserName),
            },
            DefaultAuthenticationTypes.ApplicationCookie);
            */
            /*
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = false, // Stores cookie on disk if true
                // TODO: Make sure we understand the ExpiresUtc vs auth config ExpireTimeSpan 
                // This seems to overwrite the auth setting for expiration.
                // So, only use if persistent.
                //ExpiresUtc = DateTime.UtcNow.AddDays(7)
            },
            identity);
            */
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
    }
}