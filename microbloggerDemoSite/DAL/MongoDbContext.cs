using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using microbloggerDemoSite.Identity;
using microbloggerDemoSite.Blog.Models;

namespace microbloggerDemoSite.DAL
{
    public class MongoDbContext : IDisposable
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private IMongoCollection<IdentityUser> _users;
        private IMongoCollection<Bucket> _buckets;

        public MongoDbContext() :
            this(ConfigurationManager.ConnectionStrings["mongodb"]?.ConnectionString,
                ConfigurationManager.AppSettings["mongodb_databaseName"])
        { }

        public MongoDbContext(string connectionString, string databaseName)
        {
            if (connectionString == null)
                throw new ArgumentNullException("connectionString");
            if (databaseName == null)
                throw new ArgumentNullException("databaseName");

            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            // TODO: set options. for one, assign id on insert?
            _users = _database.GetCollection<IdentityUser>("User");
            _buckets = _database.GetCollection<Bucket>("Bucket");
        }

        public IMongoCollection<Bucket> Buckets
        {
            get
            {
                return _buckets;
            }
        }

        public IMongoCollection<IdentityUser> Users
        {
            get
            {
                return _users;
            }
        }

        public void Dispose()
        {
            // Not needed for MongoDB.
        }

        public static MongoDbContext Create()
        {
            return new MongoDbContext();
        }
    }
}
