using Newtonsoft.Json;
using SynchronousShops.Integration.Tests.Data;
using SynchronousShops.Libraries.Extensions;
using SynchronousShops.Servers.API.Controllers.Identity.Dtos;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SynchronousShops.Integration.Tests.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> AuthenticateAsAdministratorAsync(this HttpClient client, ITestOutputHelper output)
        {
            return await client.AuthenticateAsync(
                TestUserDataBuilder.AdministratorEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<HttpResponseMessage> AuthenticateAsManagerAsync(this HttpClient client, ITestOutputHelper output)
        {
            return await client.AuthenticateAsync(
                TestUserDataBuilder.ManagerEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<HttpResponseMessage> AuthenticateAsUserAsync(this HttpClient client, ITestOutputHelper output)
        {
            return await client.AuthenticateAsync(
                TestUserDataBuilder.UserEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<HttpResponseMessage> AuthenticateAsAsync(this HttpClient client, string email, string password, ITestOutputHelper output)
        {
            return await client.AuthenticateAsync(
                email,
                password,
                output
            );
        }

        public static void AuthenticateAsAnonymous(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = null;
        }

        #region Private

        private static async Task<HttpResponseMessage> AuthenticateAsync(this HttpClient client, string email, string password, ITestOutputHelper output)
        {
            var requestData = new LoginRequestDto { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            output.WriteLine($"METHOD POST, url:'{Libraries.Constants.Api.V1.Authentication.Login}' dto:'{requestData.ToJson()}'");
            var response = await client.PostAsync(Libraries.Constants.Api.V1.Authentication.Login, content);

            var dto = await response.ConvertToAsync<LoginResponseDto>(output);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dto.Token);
            return response;
        }
        #endregion
    }
}
