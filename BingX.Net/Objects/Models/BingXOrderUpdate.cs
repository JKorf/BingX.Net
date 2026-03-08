using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Enums;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Order update
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXOrderUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// ["<c>s</c>"] Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>C</c>"] Client order id
        /// </summary>
        [JsonPropertyName("C")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>S</c>"] Side
        /// </summary>
        [JsonPropertyName("S")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>o</c>"] Type
        /// </summary>
        [JsonPropertyName("o")]
        public OrderType Type { get; set; }
        /// <summary>
        /// ["<c>p</c>"] Order price
        /// </summary>
        [JsonPropertyName("p")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>q</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("q")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// ["<c>x</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("x")]
        public string EventType { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>X</c>"] Order status
        /// </summary>
        [JsonPropertyName("X")]
        public OrderStatus Status { get; set; }
        /// <summary>
        /// ["<c>i</c>"] Order id
        /// </summary>
        [JsonPropertyName("i")]
        public long OrderId { get; set; }
        /// <summary>
        /// ["<c>l</c>"] Quantity of the last fill for this order
        /// </summary>
        [JsonPropertyName("l")]
        public decimal? LastFillQuantity { get; set; }
        /// <summary>
        /// ["<c>L</c>"] Price of the last fill for this order
        /// </summary>
        [JsonPropertyName("L")]
        public decimal? LastFillPrice { get; set; }
        /// <summary>
        /// ["<c>z</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("z")]
        public decimal? QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>n</c>"] Fee paid
        /// </summary>
        [JsonPropertyName("n")]
        public decimal? Fee { get; set; }
        /// <summary>
        /// ["<c>N</c>"] Fee asset
        /// </summary>
        [JsonPropertyName("N")]
        public string? FeeAsset { get; set; }
        /// <summary>
        /// ["<c>T</c>"] Update time
        /// </summary>
        [JsonPropertyName("T")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>O</c>"] Creation time
        /// </summary>
        [JsonPropertyName("O")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>t</c>"] Trade id
        /// </summary>
        [JsonPropertyName("t")]
        public long TradeId { get; set; }
        /// <summary>
        /// ["<c>Z</c>"] Quantity filled in quote asset
        /// </summary>
        [JsonPropertyName("Z")]
        public decimal? VolumeFilled { get; set; }
        /// <summary>
        /// ["<c>Y</c>"] Last fill value
        /// </summary>
        [JsonPropertyName("Y")]
        public decimal? LastFillValue { get; set; }
        /// <summary>
        /// ["<c>Q</c>"] Original quote order quantity
        /// </summary>
        [JsonPropertyName("Q")]
        public decimal? QuoteOrderQuantity { get; set; }
    }
}
