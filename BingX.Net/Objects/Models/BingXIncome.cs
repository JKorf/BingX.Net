using BingX.Net.Enums;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Income transaction
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXIncome
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("incomeType")]
        public IncomeType IncomeType { get; set; }
        /// <summary>
        /// Income
        /// </summary>
        [JsonPropertyName("income")]
        public decimal Income { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Additional info
        /// </summary>
        [JsonPropertyName("info")]
        public string Info { get; set; } = string.Empty;
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("tranId")]
        public string? TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonPropertyName("tradeId")]
        public string? TradeId { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
    }
}
