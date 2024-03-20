using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Perpetual futures contract info
    /// </summary>
    public record BingXContract
    {
        /// <summary>
        /// Contract id
        /// </summary>
        [JsonPropertyName("contractId")]
        public string ContractId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Size of a single contract
        /// </summary>
        [JsonPropertyName("size")]
        public decimal ContractSize { get; set; }
        /// <summary>
        /// Quantity precision
        /// </summary>
        [JsonPropertyName("quantityPrecision")]
        public int QuantityPrecision { get; set; }
        /// <summary>
        /// Price precision
        /// </summary>
        [JsonPropertyName("pricePrecision")]
        public int PricePrecision { get; set; }
        /// <summary>
        /// Transaction fee rate
        /// </summary>
        [JsonPropertyName("feeRate")]
        public decimal FeeRate { get; set; }
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonPropertyName("tradeMinLimit")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// Max long leverage
        /// </summary>
        [JsonPropertyName("maxLongLeverage")]
        public decimal MaxLongLeverage { get; set; }
        /// <summary>
        /// Max short leverage
        /// </summary>
        [JsonPropertyName("maxShortLeverage")]
        public decimal MaxShortLeverage { get; set; }
        /// <summary>
        /// Settlement and margin currency asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// Contract trading asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Contract status
        /// </summary>
        [JsonPropertyName("status")]
        public bool Online { get; set; }
        /// <summary>
        /// Can open positions from API
        /// </summary>
        [JsonPropertyName("apiStateOpen")]
        public bool ApiCanOpen { get; set; }
        /// <summary>
        /// Can close positions from API
        /// </summary>
        [JsonPropertyName("apiStateClose")]
        public bool ApiCanClose { get; set; }
    }
}
