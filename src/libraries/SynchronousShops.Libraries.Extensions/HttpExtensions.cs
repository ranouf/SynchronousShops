using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SynchronousShops.Libraries.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<T> ConvertToAsync<T>(this HttpResponseMessage response) where T : class
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
