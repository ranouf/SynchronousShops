using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SynchronousShops.Domains.Core.Shops.Etsy;
using SynchronousShops.Domains.Core.Shops.Etsy.Entities;
using SynchronousShops.Domains.Infrastructure.Shops.Etsy.Configuration;
using SynchronousShops.Domains.Infrastructure.Shops.Etsy.Exceptions;
using SynchronousShops.Domains.Infrastructure.Shops.Etsy.Extensions;
using SynchronousShops.Libraries.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SynchronousShops.Domains.Infrastructure.Shops.Etsy
{
    public class EtsyShopService : IEtsyShopService
    {
        private const string EtsyOpenAPIV3Url = "https://api.etsy.com/v3";

        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        private readonly EtsySettings _etsySettings;
        private readonly ILogger<EtsyShopService> _logger;

        public EtsyShopService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IOptions<EtsySettings> etsySettings,
            ILogger<EtsyShopService> logger
        )
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
            _etsySettings = etsySettings.Value;
            _logger = logger;
        }

        #region Ping
        public async Task<bool> PingAsync()
        {
            var response = await _httpClient
                .AddApiKeyToHeader(_etsySettings.ApiKey)
                .GetAsync(EtsyOpenAPIV3Url + "/application/openapi-ping");
            return response.IsSuccessStatusCode;
        }
        #endregion

        #region OAuth
        public (Uri uri, string codeVerifier, string state) GenerateRequestAnAuthorizationCodeUri()
        {
            /* Documentation: https://developers.etsy.com/documentation/essentials/authentication/#step-1-request-an-authorization-code
             * Url: GET https://www.etsy.com/oauth/connect?
             *         response_type=code
             *         &redirect_uri=https://www.example.com/some/location
             *         &scope=transactions_r%20transactions_w
             *         &client_id=1aa2bb33c44d55eeeeee6fff
             *         &state=superstate
             *         &code_challenge=DSWlW2Abh-cf8CeLL8-g3hQ2WQyYdKyiu83u_s7nRhI
             *         &code_challenge_method=S256
             */
            var redirectUrl = GetRedirectUrl();
            var state = GenerateState();
            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);

            var uri = new Uri("https://www.etsy.com/oauth/connect")
                .AddQueryStringParameter("response_type", "code")
                .AddQueryStringParameter("redirect_uri", redirectUrl)
                .AddQueryStringParameter("scope", "listings_r listings_w shops_r")
                .AddQueryStringParameter("client_id", _etsySettings.ApiKey)
                .AddQueryStringParameter("state", state)
                .AddQueryStringParameter("code_challenge", codeChallenge)
                .AddQueryStringParameter("code_challenge_method", "S256");

            return (
                uri,
                codeVerifier,
                state
            );

            static string GenerateState()
            {
                var result = Guid.NewGuid().ToString();
                return result;
            }

            static string GenerateCodeVerifier()
            {
                var rng = RandomNumberGenerator.Create();
                var bytes = new byte[32];
                rng.GetBytes(bytes);

                // It is recommended to use a URL-safe string as code_verifier.
                // See section 4 of RFC 7636 for more details.
                var result = Convert.ToBase64String(bytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
                return result;
            }

            static string GenerateCodeChallenge(string codeVerifier)
            {
                using var sha256 = SHA256.Create();
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
                var result = Convert.ToBase64String(challengeBytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
                return result;
            }
        }

        public async Task<OAuthToken> RequestAccessTokenAsync(string authorizationCode, string codeVerifier)
        {
            /* Documentation: https://developers.etsy.com/documentation/essentials/authentication/#step-3-request-an-access-token
             * Url: POST https://api.etsy.com/v3/public/oauth/token?
             *         grant_type=authorization_code
             *         &client_id=1aa2bb33c44d55eeeeee6fff
             *         &redirect_uri=https://www.example.com/some/location
             *         &code=bftcubu-wownsvftz5kowdmxnqtsuoikwqkha7_4na3igu1uy-ztu1bsken68xnw4spzum8larqbry6zsxnea4or9etuicpra5zi
             *         &code_verifier=vvkdljkejllufrvbhgeiegrnvufrhvrffnkvcknjvfid
             */

            var redirectUrl = GetRedirectUrl();

            var requestUri = new Uri(EtsyOpenAPIV3Url + "/public/oauth/token");
            var response = await _httpClient.PostAsJsonAsync(
                requestUri,
                new
                {
                    grant_type = "authorization_code",
                    client_id = _etsySettings.ApiKey,
                    redirect_uri = redirectUrl,
                    code = authorizationCode,
                    code_verifier = codeVerifier
                }
            );
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.ConvertToAsync<Error>();
                if (error.ErrorDescription == "code is expired")
                {
                    throw new CodeExpiredException();
                }
                throw new Exception(error.ErrorDescription);
            }
            var result = await response.ConvertToAsync<OAuthToken>();
            return result;
        }

        public async Task<OAuthToken> RefreshAccessTokenAsync(string refreshToken)
        {
            /* Documentation: https://developers.etsy.com/documentation/essentials/authentication/#step-3-request-an-access-token
             * Url: POST https://api.etsy.com/v3/public/oauth/token?
             *         grant_type=refresh_token
             *         &client_id=1aa2bb33c44d55eeeeee6fff
             *         &refresh_token=12345678.JNGIJtvLmwfDMhlYoOJl8aLR1BWottyHC6yhNcET-eC7RogSR5e1GTIXGrgrelWZalvh3YvvyLfKYYqvymd-u37Sjtx
             * 
             */

            var requestUri = new Uri(EtsyOpenAPIV3Url + "/public/oauth/token");
            var response = await _httpClient.PostAsJsonAsync(
                requestUri,
                new
                {
                    grant_type = "refresh_token",
                    client_id = _etsySettings.ApiKey,
                    refresh_token = refreshToken
                }
            );
            var result = await response.ConvertToAsync<OAuthToken>();
            return result;
        }
        #endregion

        #region Users
        public async Task<Me> GetMeAsync(OAuthToken token)
        {
            var response = await _httpClient
                .AddApiKeyToHeader(_etsySettings.ApiKey)
                .AddAccessTokenToHeader(token.AccessToken)
                .GetAsync(EtsyOpenAPIV3Url + "/application/users/me");
            if (!response.IsSuccessStatusCode)
            {
                // TODO handle Token Expired
            }
            var result = await response.ConvertToAsync<Me>();
            return result;
        }
        #endregion

        public async Task<GetAllListings> GetAllListingsAsync(long shopId, int limit, int offset)
        {
            // https://openapi.etsy.com/v3/application/shops/{shop_id}/listings/active
            var uri = new Uri(EtsyOpenAPIV3Url)
                .Append("")
                .Append($"/application/shops/{shopId}/listings/active")
                .AddQueryStringParameter(nameof(limit), $"{limit}")
                .AddQueryStringParameter(nameof(offset), $"{offset}");

            var response = await _httpClient
                .AddApiKeyToHeader(_etsySettings.ApiKey)
                .GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                // TODO handle Token Expired
            }
            var result = await response.ConvertToAsync<GetAllListings>();
            return result;
        }

        #region Private 

        private string GetRedirectUrl()
        {
            var result = new Uri($"{_httpContext.Request.Scheme}://{_httpContext.Request.Host.Value}")
                .Append(Libraries.Constants.Api.V1.Etsy.Redirect)
                .AbsoluteUri;
            return result;
        }

        #endregion

    }
}
