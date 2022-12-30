using Newtonsoft.Json;
using SynchronousShops.Domains.Core.Identity.Entities;
using SynchronousShops.Domains.Core.Shops.Etsy.Entities;
using SynchronousShops.Libraries.Extensions;

namespace SynchronousShops.Domains.Core.Identity.Extensions
{
    public static class UserExtensions
    {
        public static void SetEtsyCodeVerifier(this User user, string codeVerifier)
        {
            user.SetUserMetadata(UserMetaDataKey.EtsyCodeVerifier, codeVerifier);
        }

        public static string GetEtsyCodeVerifier(this User user)
        {
            return user.GetUserMetadata(UserMetaDataKey.EtsyCodeVerifier);
        }

        public static void SetEtsyState(this User user, string state)
        {
            user.SetUserMetadata(UserMetaDataKey.EtsyState, state);
        }

        public static string GetEtsyState(this User user)
        {
            return user.GetUserMetadata(UserMetaDataKey.EtsyState);
        }

        public static void SetEtsyOAuthToken(this User user, OAuthToken oAuthToken)
        {
            user.SetUserMetadata(UserMetaDataKey.EtsyOAuthToken, oAuthToken.ToJson());
        }

        public static OAuthToken GetEtsyOAuthToken(this User user)
        {
            return JsonConvert.DeserializeObject<OAuthToken>(user.GetUserMetadata(UserMetaDataKey.EtsyOAuthToken));
        }

        public static void SetEtsyUserId(this User user, long userId)
        {
            user.SetUserMetadata(UserMetaDataKey.EtsyUserId, userId.ToString());
        }

        public static long GetEtsyUserId(this User user)
        {
            return long.Parse(user.GetUserMetadata(UserMetaDataKey.EtsyUserId));
        }

        public static void SetEtsyShopId(this User user, long shopId)
        {
            user.SetUserMetadata(UserMetaDataKey.EtsyShopId, shopId.ToString());
        }

        public static long GetEtsyShopId(this User user)
        {
            return long.Parse(user.GetUserMetadata(UserMetaDataKey.EtsyShopId));
        }
    }
}
