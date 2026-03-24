using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>contractId</c>"] Contract id
        /// </summary>
        [JsonPropertyName("contractId")]
        public string ContractId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>displayName</c>"] Display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>size</c>"] Size of a single contract
        /// </summary>
        [JsonPropertyName("size")]
        public decimal ContractSize { get; set; }
        /// <summary>
        /// ["<c>quantityPrecision</c>"] Quantity precision
        /// </summary>
        [JsonPropertyName("quantityPrecision")]
        public int QuantityPrecision { get; set; }
        /// <summary>
        /// ["<c>pricePrecision</c>"] Price precision
        /// </summary>
        [JsonPropertyName("pricePrecision")]
        public int PricePrecision { get; set; }
        /// <summary>
        /// ["<c>feeRate</c>"] Transaction fee rate
        /// </summary>
        [JsonPropertyName("feeRate")]
        public decimal FeeRate { get; set; }
        /// <summary>
        /// ["<c>makerFeeRate</c>"] Maker fee rate
        /// </summary>
        [JsonPropertyName("makerFeeRate")]
        public decimal MakerFeeRate { get; set; }
        /// <summary>
        /// ["<c>takerFeeRate</c>"] Taker fee rate
        /// </summary>
        [JsonPropertyName("takerFeeRate")]
        public decimal TakerFeeRate { get; set; }
        /// <summary>
        /// ["<c>tradeMinQuantity</c>"] Min order quantity
        /// </summary>
        [JsonPropertyName("tradeMinQuantity")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>tradeMinUSDT</c>"] Trade minimal value in USDT
        /// </summary>
        [JsonPropertyName("tradeMinUSDT")]
        public decimal MinOrderValue { get; set; }
        /// <summary>
        /// ["<c>maxLongLeverage</c>"] Max long leverage
        /// </summary>
        [JsonPropertyName("maxLongLeverage")]
        public decimal MaxLongLeverage { get; set; }
        /// <summary>
        /// ["<c>maxShortLeverage</c>"] Max short leverage
        /// </summary>
        [JsonPropertyName("maxShortLeverage")]
        public decimal MaxShortLeverage { get; set; }
        /// <summary>
        /// ["<c>currency</c>"] Settlement and margin currency asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asset</c>"] Contract trading asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] 1: Online, 25:Suspended. Different unknown values have been observed. 
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }
        /// <summary>
        /// ["<c>apiStateOpen</c>"] Can open positions from API
        /// </summary>
        [JsonPropertyName("apiStateOpen")]
        public bool ApiCanOpen { get; set; }
        /// <summary>
        /// ["<c>apiStateClose</c>"] Can close positions from API
        /// </summary>
        [JsonPropertyName("apiStateClose")]
        public bool ApiCanClose { get; set; }
        /// <summary>
        /// ["<c>ensureTrigger</c>"] Whether to support guaranteed stop loss.
        /// </summary>
        [JsonPropertyName("ensureTrigger")]
        public bool EnsureTrigger { get; set; }
        /// <summary>
        /// ["<c>triggerFeeRate</c>"] The fee rate for guaranteed stop loss.
        /// </summary>
        [JsonPropertyName("triggerFeeRate")]
        public decimal TriggerFeeRate { get; set; }
        /// <summary>
        /// ["<c>brokerState</c>"] Whether broker user transaction are prohibited
        /// </summary>
        [JsonPropertyName("brokerState")]
        public bool BrokerProhibited { get; set; }
        /// <summary>
        /// ["<c>launchTime</c>"] Launch time
        /// </summary>
        [JsonPropertyName("launchTime")]
        public DateTime? LaunchTime { get; set; }
        /// <summary>
        /// ["<c>maintainTime</c>"] Time of planned maintenance
        /// </summary>
        [JsonPropertyName("maintainTime")]
        public DateTime? MaintenanceTime { get; set; }
        /// <summary>
        /// ["<c>offTime</c>"] Time of planned offline
        /// </summary>
        [JsonPropertyName("offTime")]
        public DateTime? OfflineTime { get; set; }
    }
}
