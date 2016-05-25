using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace microbloggerDemoSite.Models.Auth
{
    public class UpdatePasswordViewModel
    {
        [Display(Name = "Current password", ShortName = "Current",
            Description = "The currently used password.")]
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New password", ShortName = "New",
            Description = "The new password to use.")]
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Length must be between 8 and 100 characters.")]
        public string Password { get; set; }

        [Display(Name = "Re-enter new password", ShortName = "New Repeat",
            Description = "Re-enter the new password to use.")]
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Length must be between 8 and 100 characters.")]
        public string PasswordRepeat { get; set; }
    }
}
