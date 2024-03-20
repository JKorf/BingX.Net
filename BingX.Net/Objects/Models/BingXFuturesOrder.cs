using System.Text.Json.Serialization;
using BingX.Net.Enums;

namespace BingX.Net.Objects.Models
{
    internal record BingXFuturesOrderWrapper
    {
        [JsonPropertyName("order")]
        public BingXFuturesOrder Order { get; set; } = null!;
    }

    /// <summary>
    /// Order info
    /// </summary>
    public record BingXFuturesOrder
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("positionSide")]
        public PositionSide? PositionSide { get; set; }
        /// <summary>
        /// Price trigger type
        /// </summary>
        [JsonPropertyName("workingType")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientOrderID")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Stop guaranteed
        /// </summary>
        [JsonPropertyName("stopGuaranteed")]
        public bool StopGuaranteed { get; set; }
    }
}
