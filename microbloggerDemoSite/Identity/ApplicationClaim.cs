using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using MongoDB.Bson.Serialization.Attributes;

namespace microbloggerDemoSite.Identity
{
    /// <summary>
    /// Custom Claim class, to allow serialization of claims to the database.
    /// </summary>
    public class ApplicationClaim
    {
        // TODO: Learn more about claims, and the various properties. I.e. issuer.
        public ApplicationClaim(string type, string value)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException("type");
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value");

            Type = type;
            Value = value;
        }

        [BsonRequired]
        public string Type { get; set; }

        [BsonRequired]
        public string Value { get; set; }

        /// <summary>
        /// Returns a new System.Security.Claims.Claim object, using the current state.
        /// </summary>
        [BsonIgnore]
        public Claim SecurityClaim
        {
            get
            {
                return new Claim(Type, Value);
            }
        }
    }
}
