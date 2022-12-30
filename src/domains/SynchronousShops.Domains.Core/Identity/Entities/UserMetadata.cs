using SynchronousShops.Libraries.Entities;
using System;

namespace SynchronousShops.Domains.Core.Identity.Entities
{
    public enum UserMetaDataKey
    {
        EtsyCodeVerifier = 1,
        EtsyState = 2,
        EtsyOAuthToken = 3,
        EtsyUserId = 4,
        EtsyShopId = 5,
    }

    public class UserMetadata : Entity, IEntity
    {
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }
        public UserMetaDataKey Key { get; private set; }
        public string Value { get; private set; }

        internal UserMetadata() { }

        public UserMetadata(User user, UserMetaDataKey key, string value)
        {
            UserId = user.Id;
            Key = key;
            Value = value;
        }

        public UserMetadata Update(string value)
        {
            Value = value;
            return this;
        }
    }
}
