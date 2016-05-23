using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using microbloggerDemoSite.Blog.Models;

namespace microbloggerDemoSite.Identity
{
    public class IdentityUser : IUser<string>
    {
        public IdentityUser()
        {
            ApplicationClaims = new List<ApplicationClaim>();
            RecentPosts = new List<Post>();
        }
        
        [HiddenInput(DisplayValue = false)]
        [BsonId]
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        
        /// <summary>
        /// Unique username.
        /// Email address.
        /// </summary>
        [Required]
        [BsonRequired]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [BsonRequired]
        [DataType(DataType.Text)]
        public string Nick { get; set; }

        [Required]
        [BsonRequired]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        [BsonIgnoreIfNull]
        public IList<ApplicationClaim> ApplicationClaims { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BucketCount { get; set; }

        [HiddenInput(DisplayValue = false)]
        //[BsonRepresentation(BsonType.DateTime)]
        public DateTime LastPost { get; set; }

        /// <summary>
        /// A cache of most recent posts made by the user.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public IList<Post> RecentPosts { get; set; }
    }
}
