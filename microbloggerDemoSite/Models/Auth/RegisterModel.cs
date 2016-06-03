using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace microbloggerDemoSite.Models.Auth
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Username (email)", Description = "Used to log in.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Username must be a valid email address.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Nick", Description = "Your public name.")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Length must be between 1 and 50 characters.")]
        public string Nick { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Length must be between 8 and 100 characters.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Password (again)")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Length must be between 8 and 100 characters.")]
        public string PasswordRepeat { get; set; }
    }
}
