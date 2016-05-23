using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Bson.Serialization.Attributes;

namespace microbloggerDemoSite.Blog.Models
{
    public class Bucket
    {
        public Bucket()
        {
            Posts = new List<Post>();
        }

        [BsonIgnore]
        public const int MAX_POSTS_COUNT = 100;

        [BsonRequired]
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string AuthorId { get; set; }

        [BsonRequired]
        public int BucketNumber { get; set; }

        [BsonRequired]
        public int PostsCount { get; set; }

        public bool Full { get; set; } // Bool default value is false. https://msdn.microsoft.com/en-gb/library/83fhsxwc.aspx

        public List<Post> Posts { get; set; }
        
        public bool AddPost(Post newPost)
        {
            if (newPost == null)
                throw new ArgumentNullException("newPost");

            bool success = false;

            if (!Full)
            {
                Posts.Add(newPost);
                PostsCount++;
                if (PostsCount >= MAX_POSTS_COUNT)
                    Full = true;
                success = true;
            }

            return success;
        }
    }
}
