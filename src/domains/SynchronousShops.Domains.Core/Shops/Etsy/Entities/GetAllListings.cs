using Newtonsoft.Json;

namespace SynchronousShops.Domains.Core.Shops.Etsy.Entities
{
    public partial class GetAllListings
    {
        [JsonProperty("count")]
        public long? Count { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("listing_id")]
        public long? ListingId { get; set; }

        [JsonProperty("user_id")]
        public long? UserId { get; set; }

        [JsonProperty("shop_id")]
        public long? ShopId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("creation_timestamp")]
        public long? CreationTimestamp { get; set; }

        [JsonProperty("created_timestamp")]
        public long? CreatedTimestamp { get; set; }

        [JsonProperty("ending_timestamp")]
        public long? EndingTimestamp { get; set; }

        [JsonProperty("original_creation_timestamp")]
        public long? OriginalCreationTimestamp { get; set; }

        [JsonProperty("last_modified_timestamp")]
        public long? LastModifiedTimestamp { get; set; }

        [JsonProperty("updated_timestamp")]
        public long? UpdatedTimestamp { get; set; }

        [JsonProperty("state_timestamp")]
        public long? StateTimestamp { get; set; }

        [JsonProperty("quantity")]
        public long? Quantity { get; set; }

        [JsonProperty("shop_section_id")]
        public long? ShopSectionId { get; set; }

        [JsonProperty("featured_rank")]
        public long? FeaturedRank { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("num_favorers")]
        public long? NumFavorers { get; set; }

        [JsonProperty("non_taxable")]
        public bool NonTaxable { get; set; }

        [JsonProperty("is_taxable")]
        public bool IsTaxable { get; set; }

        [JsonProperty("is_customizable")]
        public bool IsCustomizable { get; set; }

        [JsonProperty("is_personalizable")]
        public bool IsPersonalizable { get; set; }

        [JsonProperty("personalization_is_required")]
        public bool PersonalizationIsRequired { get; set; }

        [JsonProperty("personalization_char_count_max")]
        public long? PersonalizationCharCountMax { get; set; }

        [JsonProperty("personalization_instructions")]
        public string PersonalizationInstructions { get; set; }

        [JsonProperty("listing_type")]
        public string ListingType { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("materials")]
        public string[] Materials { get; set; }

        [JsonProperty("shipping_profile_id")]
        public long? ShippingProfileId { get; set; }

        [JsonProperty("return_policy_id")]
        public long? ReturnPolicyId { get; set; }

        [JsonProperty("processing_min")]
        public long? ProcessingMin { get; set; }

        [JsonProperty("processing_max")]
        public long? ProcessingMax { get; set; }

        [JsonProperty("who_made")]
        public string WhoMade { get; set; }

        [JsonProperty("when_made")]
        public string WhenMade { get; set; }

        [JsonProperty("is_supply")]
        public bool IsSupply { get; set; }

        [JsonProperty("item_weight")]
        public long? ItemWeight { get; set; }

        [JsonProperty("item_weight_unit")]
        public string ItemWeightUnit { get; set; }

        [JsonProperty("item_length")]
        public long? ItemLength { get; set; }

        [JsonProperty("item_width")]
        public long? ItemWidth { get; set; }

        [JsonProperty("item_height")]
        public long? ItemHeight { get; set; }

        [JsonProperty("item_dimensions_unit")]
        public string ItemDimensionsUnit { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("style")]
        public string[] Style { get; set; }

        [JsonProperty("file_data")]
        public string FileData { get; set; }

        [JsonProperty("has_variations")]
        public bool HasVariations { get; set; }

        [JsonProperty("should_auto_renew")]
        public bool ShouldAutoRenew { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("taxonomy_id")]
        public long? TaxonomyId { get; set; }
    }

    public partial class Price
    {
        [JsonProperty("amount")]
        public long? Amount { get; set; }

        [JsonProperty("divisor")]
        public long? Divisor { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
    }
}
