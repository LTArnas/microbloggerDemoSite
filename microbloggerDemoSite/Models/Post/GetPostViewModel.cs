using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using microbloggerDemoSite.Identity;

namespace microbloggerDemoSite.Models.Post
{
    public class GetPostViewModel
    {
        public IdentityUser Author { get; set; }

        public Blog.Models.Post Post { get; set; }
    }
}
