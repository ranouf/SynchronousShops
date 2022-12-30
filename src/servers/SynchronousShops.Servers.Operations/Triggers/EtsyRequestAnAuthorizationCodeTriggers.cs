using Microsoft.Extensions.Logging;

namespace SynchronousShops.Servers.Operations.Triggers
{
    public class EtsyRequestAnAuthorizationCodeTriggers
    {
        private readonly ILogger _logger;

        public EtsyRequestAnAuthorizationCodeTriggers(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EtsyRequestAnAuthorizationCodeTriggers>();
        }

        //[Function("EtsyRequestAnAuthorizationCodeTriggers")]
        //public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        //{
        //    return new RedirectResult("https://blog.yannickreekmans.be", true);
        //}
    }
}
