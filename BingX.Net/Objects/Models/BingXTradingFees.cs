using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trading fee info
    /// </summary>
    public record BingXTradingFees
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
