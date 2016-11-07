using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.DAL;
using microbloggerDemoSite.Blog.Models;

namespace microbloggerDemoSite.Blog
{
    public class BlogManager : IDisposable // Required for Owin's CreatePerOwinContext().
    {
        // TODO: check how we use the usermanager, and try to remove it.
        private UserManager<IdentityUser> _userManager;
        private MongoDbContext _db;

        public BlogManager(UserManager<IdentityUser> userManager, MongoDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public Post AddPost(string userId, Post newPost)
        {
            if (newPost == null)
                throw new ArgumentNullException("newPost");

            var bucketFilter = Builders<Bucket>.Filter.Eq((Bucket x) => x.AuthorId, userId) &
                Builders<Bucket>.Filter.Eq((Bucket x) => x.Full, false);
            var userFilter = Builders<IdentityUser>.Filter.Eq((IdentityUser x) => x.Id, userId);

            Bucket bucket = null;
            using (IAsyncCursor<Bucket> cursor = _db.Buckets.FindSync(bucketFilter))
            {
                while (cursor.MoveNext())
                {
                    foreach (Bucket searchBucket in cursor.Current)
                    {
                        bucket = searchBucket;
                        break;
                    }
                    if (bucket != null)
                        break;
                }
            }

            if (bucket == null) // we need a new bucket
            {
                var bucketCountUpdate = Builders<IdentityUser>.Update.Inc((IdentityUser u) => u.BucketCount, 1);
                IdentityUser user = _db.Users.FindOneAndUpdate(userFilter, bucketCountUpdate, new FindOneAndUpdateOptions<IdentityUser, IdentityUser> { ReturnDocument = ReturnDocument.After });
                //IdentityUser user = _db.Users.Find(userFilter).First();
                bucket = new Bucket();
                bucket.AuthorId = userId;
                bucket.BucketNumber = user.BucketCount;
                bucket.AddPost(newPost);
                _db.Buckets.InsertOne(bucket);
            }
            else
            {
                bucket.AddPost(newPost);
                _db.Buckets.FindOneAndReplace(bucketFilter, bucket);
            }

            // Adjust recent posts list for the user.
            var recentPostsSort = Builders<Post>.Sort.Descending(x => x.PublishDate);
            var recentPostUpdate = Builders<IdentityUser>.Update.PushEach((IdentityUser x) => x.RecentPosts, new[] { newPost }, 10, null, recentPostsSort);
            var lastPostUpdate = Builders<IdentityUser>.Update.CurrentDate((IdentityUser x) => x.LastPost);
            // With this combined update, we could reorganize this, so we do a single user update for the whole function. (see new bucket creation.)
            var combinedUpdate = Builders<IdentityUser>.Update.Combine(recentPostUpdate, lastPostUpdate);
            _db.Users.FindOneAndUpdate(userFilter, combinedUpdate);

            return newPost;
        }

        public Bucket FindBucketByNumber(string userId, int bucketNumber)
        {
            Bucket result = null;

            var filter = Builders<Bucket>.Filter.Eq((Bucket x) => x.AuthorId, userId) &
                Builders<Bucket>.Filter.Eq((Bucket x) => x.BucketNumber, bucketNumber);

            using (IAsyncCursor<Bucket> cursor = _db.Buckets.FindSync(filter))
            {
                while (cursor.MoveNext())
                {
                    foreach (Bucket bucket in cursor.Current)
                    {
                        result = bucket;
                        break;
                    }
                    if (result != null)
                        break;
                }
            }

            return result;
        }

        public Post FindPostById(string userId, string id)
        {
            Post result = null;

            var filter = Builders<Bucket>.Filter.Eq((Bucket x) => x.AuthorId, userId);
            using (IAsyncCursor<Bucket> cursor = _db.Buckets.FindSync(filter))
            {
                while (cursor.MoveNext())
                {
                    foreach (Bucket bucket in cursor.Current)
                    {
                        int index = bucket.Posts.FindIndex(x => x.Id == id);
                        if (index > -1)
                        {
                            result = bucket.Posts[index];
                            break;
                        }
                    }
                    if (result != null)
                        break;
                }
            }

            return result;
        }

        public void Dispose() { } // Nothing to dispose.
    }
}
