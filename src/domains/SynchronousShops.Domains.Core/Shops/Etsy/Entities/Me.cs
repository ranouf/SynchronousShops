using Newtonsoft.Json;

namespace SynchronousShops.Domains.Core.Shops.Etsy.Entities
{
    public class Me
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("shop_id")]
        public long ShopId { get; set; }
    }
}
