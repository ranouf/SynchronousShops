using SynchronousShops.Domains.Core.Shops.Etsy.Entities;
using System;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Core.Shops.Etsy
{
    public interface IEtsyShopService
    {
        (Uri uri, string codeVerifier, string state) GenerateRequestAnAuthorizationCodeUri();
        Task<bool> PingAsync();
        Task<OAuthToken> RequestAccessTokenAsync(string authorizationCode, string codeVerifier);
        Task<OAuthToken> RefreshAccessTokenAsync(string refreshToken);
        Task<Me> GetMeAsync(OAuthToken token);
        Task<GetAllListings> GetAllListingsAsync(long shopId, int limit, int offset);
    }
}