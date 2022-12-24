using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SynchronousShops.Servers.Operations.Triggers
{
    public class MessagerTriggers
    {
        private readonly ILogger<MessagerTriggers> _logger;

        public MessagerTriggers(
            ILogger<MessagerTriggers> logger
        )
        {
            _logger = logger;
        }

        [Function("QueueMessage")]
        public async Task QueueMessageAsync(
            [TimerTrigger("%MessageQueuerOccurence%", RunOnStartup = true)] TimerInfo timer
        )
        {
            try
            {
                //Nothing
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
            }
        }
    }
}
