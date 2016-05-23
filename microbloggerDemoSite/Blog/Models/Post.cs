using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace microbloggerDemoSite.Blog.Models
{
    public class Post
    {
        public Post()
        {
            Id = ObjectId.GenerateNewId().ToString();
            PublishDate = DateTime.Now;
        }

        [HiddenInput(DisplayValue = false)]
        [BsonRequired]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Publish Date", Description ="Publish date", ShortName = "Date")]
        [BsonRequired]
        public DateTime PublishDate { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(maximumLength : 500, MinimumLength = 1, ErrorMessage = "Content length must be between 1 and 500 characters.")]
        [Display(Name = "Content", Description = "The content of the post.", ShortName = "Content")]
        [Required]
        [BsonRequired]
        public string Content { get; set; }
    }
}
