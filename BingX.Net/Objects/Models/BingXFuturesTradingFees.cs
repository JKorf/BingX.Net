using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXFuturesTradingFeesWrapper
    {
        [JsonPropertyName("commission")]
        public BingXFuturesTradingFees Rates { get; set; } = null!;
    }

    /// <summary>
    /// Futures trading fee rates
    /// </summary>
    public record BingXFuturesTradingFees
    {
        /// <summary>
        /// Taker fee rate
        /// </summary>
        [JsonPropertyName("takerCommissionRate")]
        public decimal TakerFeeRate { get; set; }
        /// <summary>
        /// Maker fee rate
        /// </summary>
        [JsonPropertyName("makerCommissionRate")]
        public decimal MakerFeeRate { get; set; }
    }
}
