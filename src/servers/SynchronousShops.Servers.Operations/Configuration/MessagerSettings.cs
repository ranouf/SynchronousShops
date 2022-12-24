using System.ComponentModel.DataAnnotations;

namespace SynchronousShops.Servers.Operations.Configuration
{
    public class MessagerSettings
    {
        [Required]
        public string QueueName { get; set; }
        [Required]
        public string MessageQueuerOccurence { get; set; }
    }
}
