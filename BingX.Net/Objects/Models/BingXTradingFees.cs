using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trading fee info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXTradingFees
    {
        /// <summary>
        /// ["<c>takerCommissionRate</c>"] Taker fee rate
        /// </summary>
        [JsonPropertyName("takerCommissionRate")]
        public decimal TakerFeeRate { get; set; }
        /// <summary>
        /// ["<c>makerCommissionRate</c>"] Maker fee rate
        /// </summary>
        [JsonPropertyName("makerCommissionRate")]
        public decimal MakerFeeRate { get; set; }
    }
}
