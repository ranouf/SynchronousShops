using Newtonsoft.Json;

namespace SynchronousShops.Domains.Core.Shops.Etsy.Entities
{
    public class Error
    {
        [JsonProperty("error")]
        public string Code { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
