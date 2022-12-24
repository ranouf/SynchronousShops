using Microsoft.AspNetCore.Http;
using StackExchange.Profiling;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.API.Middlewares
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (MiniProfiler.Current.Step("HttpRequest"))
            {
                await _next(context);
            }
        }
    }
}
