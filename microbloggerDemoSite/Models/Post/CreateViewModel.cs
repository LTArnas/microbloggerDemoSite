using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace microbloggerDemoSite.Models.Post
{
    public class CreateViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cannot be empty.")]
        public string Content { get; set; }
    }
}
