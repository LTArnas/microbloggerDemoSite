using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using MongoDB.Bson;
using microbloggerDemoSite.DAL;

namespace microbloggerDemoSite.Identity
{
    class UserStore: 
        IUserStore<IdentityUser>,
        IQueryableUserStore<IdentityUser>, 
        IUserPasswordStore<IdentityUser>,
        IUserClaimStore<IdentityUser>
    {
        private readonly MongoDbContext _context;

        public UserStore(MongoDbContext context)
        {
            _context = context;
        }

        #region IQueryableUserStore
        public IQueryable<IdentityUser> Users
        {
            get
            {
                return _context.Users.AsQueryable();
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            // Not needed for mongodb
        }
        #endregion

        #region IUserStore
        public async Task CreateAsync(IdentityUser user)
        {
            await _context.Users.InsertOneAsync(user);
        }

        public async Task DeleteAsync(IdentityUser user)
        {
            var filter = Builders<IdentityUser>.Filter.Eq((IdentityUser u) => u.Id, user.Id); //|
                //Builders<IdentityUser>.Filter.Eq((IdentityUser u) => u.UserName, user.UserName);
            DeleteResult result =  await _context.Users.DeleteOneAsync(filter);
            // TODO: handle status result.
        }

        public async Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            // TODO: Think some more about this. Now replaces, not really an update.
            // But maybe that is how a model-to-model update should work?
            // Should use other, more specific methods for partial updates?
            var filter = Builders<IdentityUser>.Filter.Eq((IdentityUser u) => u.Id, user.Id); //|
                //Builders<IdentityUser>.Filter.Eq((IdentityUser u) => u.UserName, user.UserName);
            
            ReplaceOneResult result = await _context.Users.ReplaceOneAsync(filter, user);
            // TODO: handle status result.
        }

        public async Task<IdentityUser> FindByIdAsync(string userId)
        {
            var filter = Builders<IdentityUser>.Filter.Eq((IdentityUser u) => u.Id, userId);
            return await FirstOrNullAsync(filter);
        }

        // Username is unique.
        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            var filter = Builders<IdentityUser>.Filter.Eq((IdentityUser u) => u.UserName, userName);
            IdentityUser result = await FirstOrNullAsync(filter);
            return result;
        }
        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrWhiteSpace(user.Password))
                return Task.FromResult<bool>(false);
            else
                return Task.FromResult<bool>(true);
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Password = passwordHash;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserClaimStore
        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");

            user.ApplicationClaims.Add(new ApplicationClaim(claim.Type, claim.Value));
            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            List<Claim> result = new List<Claim>();

            foreach (ApplicationClaim item in user.ApplicationClaims)
            {
                result.Add(item.SecurityClaim);
            }

            return Task.FromResult(result as IList<Claim>);
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (claim == null)
                throw new ArgumentNullException("claim");
            // TODO: check result, act accordingly.
            user.ApplicationClaims.Remove(new ApplicationClaim(claim.Type, claim.Value));
            return Task.FromResult(0);
        }
        #endregion

        private async Task<IdentityUser> FirstOrNullAsync(FilterDefinition<IdentityUser> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            IdentityUser result = null;

            using (IAsyncCursor<IdentityUser> cursor = await _context.Users.FindAsync(filter))
            {
                if (await cursor.MoveNextAsync())
                    if (cursor.Current.Any())
                        result = cursor.Current.First();
            }

            return result;
        }
    }
}
