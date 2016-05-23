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
        [BsonRequired]
        public DateTime PublishDate { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(maximumLength : 500, MinimumLength = 1)]
        [Required]
        [BsonRequired]
        public string Content { get; set; }
    }
}
