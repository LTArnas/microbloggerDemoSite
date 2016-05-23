using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Blog.Models;

namespace microbloggerDemoSite.Models.Post
{
    public class ListPostsViewModel
    {
        public IdentityUser Author { get; set; }

        public Bucket Bucket { get; set; }
    }
}
