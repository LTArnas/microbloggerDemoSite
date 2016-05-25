using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace microbloggerDemoSite.Models.User
{
    public class UpdateUserViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nick", 
            Description = "This will appear as your name.", 
            Prompt = "What would you like to call yourself?", 
            ShortName = "Nick")]
        [StringLength(50, MinimumLength = 1,
            ErrorMessage = "Must be between 1 and 50 characters long.")]
        public string Nick { get; set; }
    }
}
