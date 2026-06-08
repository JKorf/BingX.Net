using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Cancel replace result
    /// </summary>
    public record BingXCancelReplaceResult
    {
        /// <summary>
        /// ["<c>cancelResult</c>"] Cancel result
        /// </summary>
        [JsonPropertyName("cancelResult")]
        public bool CancelResult { get; set; }
        /// <summary>
        /// ["<c>cancelMsg</c>"] Cancel error message
        /// </summary>
        [JsonPropertyName("cancelMsg")]
        public string? CancelMessage { get; set; }
        /// <summary>
        /// ["<c>cancelResponse</c>"] The canceled order
        /// </summary>
        [JsonPropertyName("cancelResponse")]
        public BingXFuturesOrderDetails? CanceledOrder { get; set; }
        /// <summary>
        /// ["<c>replaceResult</c>"] Replace result
        /// </summary>
        [JsonPropertyName("replaceResult")]
        public bool ReplaceResult { get; set; }
        /// <summary>
        /// ["<c>replaceMsg</c>"] Replace error message
        /// </summary>
        [JsonPropertyName("replaceMsg")]
        public string? ReplaceMessage { get; set; }
        /// <summary>
        /// ["<c>newOrderResponse</c>"] The new order
        /// </summary>
        [JsonPropertyName("newOrderResponse")]
        public BingXFuturesOrder? NewOrder { get; set; }
    }
}
