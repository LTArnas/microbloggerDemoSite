using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Web.Routing;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Blog;

namespace microbloggerDemoSite.Controllers
{
    /// <summary>
    /// Base controller class for the application.
    /// All our MVC controllers should inherit from this controller.
    /// Provides common functionality and properties.
    /// </summary>
    public class BaseController : Controller
    {
        // TODO: implement constructor to allow for DI on these.
        private UserManager<IdentityUser> _userManager;
        private IAuthenticationManager _authManager;
        private BlogManager _blogManager;
/*
        // TODO: This might not be correct way of doing this. may need to override special method, Initialize? OnActionExecute?
        public BaseController() :
            base()
        {
        }
        */
        // TODO: have a private authManager variable. use override initialize method on controller.
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                //return HttpContext.GetOwinContext().Authentication;
                return _authManager ?? HttpContext.GetOwinContext().Authentication;
            }
            private set
            {
                _authManager = value;
            }
        }

        public UserManager<IdentityUser> UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager<IdentityUser>>();
            }
            protected set
            {
                _userManager = value;
            }
        }

        public BlogManager BlogManager
        {
            get
            {
                return _blogManager ?? HttpContext.GetOwinContext().Get<BlogManager>();
            }
            private set
            {
                _blogManager = value;
            }
        }

        public ActionResult RedirectLocal(string targetUrl, string backupLocalUrl = "/")
        {
            #region Argument Checks
            if (string.IsNullOrWhiteSpace(backupLocalUrl))
            {
                if (backupLocalUrl == null)
                    throw new ArgumentNullException("backupLocalUrl");
                else
                    throw new ArgumentException("Empty or whitespace.", "backupLocalUrl");
            }

            if (!Url.IsLocalUrl(backupLocalUrl))
                throw new ArgumentException("Not a local URL.", "backupLocalUrl");

            // TODO: reconsider this behaviour.
            //if (string.IsNullOrWhiteSpace(targetUrl))
            //    targetUrl = backupLocalUrl;
            #endregion

            if (Url.IsLocalUrl(targetUrl))
                return Redirect(targetUrl);
            else
                return Redirect(backupLocalUrl);
        }
    }
}