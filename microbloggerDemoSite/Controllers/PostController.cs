using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Models.Post;
using microbloggerDemoSite.Blog.Models;

namespace microbloggerDemoSite.Controllers
{
    // TODO: check authorization/authentication. Does this guarantee authentication?
    [Authorize]
    public class PostController : BaseController
    {
        // GET: Post
        public ActionResult Index()
        {
            return RedirectLocal(Url.Action("Get"));
        }

        public ActionResult Get(string userId, string postId)
        {
            Post result;

            if (string.IsNullOrWhiteSpace(userId))
                userId = User.Identity.GetUserId();

            result = BlogManager.FindPostById(userId, postId);
            
            // Null reference is handled by the view, as 'not found'.
            return View(result);
        }

        public ActionResult Create(Post model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Post(Post model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                model.PublishDate = DateTime.Now;
                Post result = BlogManager.AddPost(userId, model);
                return RedirectLocal(Url.Action("Get", new { postId = result?.Id }));
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult List(string userId, int page = 1)
        {
            Bucket result;

            if (string.IsNullOrWhiteSpace(userId))
                userId = User.Identity.GetUserId();
            
            result = BlogManager.FindBucketByNumber(userId, page);

            return View(result);
        }
    }
}