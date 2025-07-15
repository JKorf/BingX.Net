using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Perpetual futures contract info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
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
        /// Display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
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
        /// Maker fee rate
        /// </summary>
        [JsonPropertyName("makerFeeRate")]
        public decimal MakerFeeRate { get; set; }
        /// <summary>
        /// Taker fee rate
        /// </summary>
        [JsonPropertyName("takerFeeRate")]
        public decimal TakerFeeRate { get; set; }
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonPropertyName("tradeMinQuantity")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// Trade minimal value in USDT
        /// </summary>
        [JsonPropertyName("tradeMinUSDT")]
        public decimal MinOrderValue { get; set; }
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
        /// 1: Online, 25:Suspended. Different unknown values have been observed. 
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }
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
        /// <summary>
        /// Whether to support guaranteed stop loss.
        /// </summary>
        [JsonPropertyName("ensureTrigger")]
        public bool EnsureTrigger { get; set; }
        /// <summary>
        /// The fee rate for guaranteed stop loss.
        /// </summary>
        [JsonPropertyName("triggerFeeRate")]
        public decimal TriggerFeeRate { get; set; }
        /// <summary>
        /// Whether broker user transaction are prohibited
        /// </summary>
        [JsonPropertyName("brokerState")]
        public bool BrokerProhibited { get; set; }
    }
}
