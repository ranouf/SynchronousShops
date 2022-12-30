using System.Net.Http;
using System.Net.Http.Headers;

namespace SynchronousShops.Domains.Infrastructure.Shops.Etsy.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddApiKeyToHeader(this HttpClient httpclient, string apiKey)
        {
            httpclient.DefaultRequestHeaders.Add(
              "x-api-key", apiKey
            );
            return httpclient;
        }

        public static HttpClient AddAccessTokenToHeader(this HttpClient httpclient, string accessToken)
        {
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return httpclient;
        }
    }
}
