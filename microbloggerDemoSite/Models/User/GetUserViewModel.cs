using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace microbloggerDemoSite.Models.User
{
    public class GetUserViewModel
    {
        public string Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        public string Nick { get; set; }

        public DateTime LastPost { get; set; }

        public IList<Blog.Models.Post> RecentPosts { get; set; }
    }
}
