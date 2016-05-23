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

        [AllowAnonymous]
        public ActionResult Get(string userId, string postId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                userId = User.Identity.GetUserId();

            Post result = BlogManager.FindPostById(userId, postId);
            IdentityUser author = UserManager.FindById(userId);

            GetPostViewModel viewModel = new GetPostViewModel
            {
                Post = result,
                Author = author
            };
            
            return View(viewModel);
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

        [AllowAnonymous]
        public ActionResult List(string userId, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(userId))
                userId = User.Identity.GetUserId();

            IdentityUser author = UserManager.FindById(userId);

            if (page < 1)
                page = 1;
            if (page > author.BucketCount)
                page = author.BucketCount;
            
            Bucket result = BlogManager.FindBucketByNumber(userId, page);

            ListPostsViewModel viewModel = new ListPostsViewModel
            {
                Author = author,
                Bucket = result
            };

            return View(viewModel);
        }
    }
}