using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Order to place
    /// </summary>
    public record BingXPlaceOrderRequest
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public OrderType Type { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonPropertyName("timeInForce"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPrice"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Quantity in quote asset
        /// </summary>
        [JsonPropertyName("quoteOrderQty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? QuoteOrderQuantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Price { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("newClientOrderId"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientOrderId { get; set; }
    }
}
