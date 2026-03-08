using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Enums;

namespace BingX.Net.Objects.Models
{
    [SerializationModel]
    internal record BingXFuturesOrderUpdateWrapper : BingXSocketUpdate
    {
        [JsonPropertyName("o")]
        public BingXFuturesOrderUpdate Data { get; set; } = null!;
    }

    /// <summary>
    /// Order update
    /// </summary>
    [SerializationModel]
    public record BingXFuturesOrderUpdate
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
        public FuturesOrderType Type { get; set; }
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
        /// ["<c>Z</c>"] Quantity filled in quote asset
        /// </summary>
        [JsonPropertyName("Z")]
        public decimal? VolumeFilled { get; set; }

        /// <summary>
        /// ["<c>sp</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("sp")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>wt</c>"] Trigger type
        /// </summary>
        [JsonPropertyName("wt")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// ["<c>ap</c>"] Average order price
        /// </summary>
        [JsonPropertyName("ap")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// ["<c>ps</c>"] Position side
        /// </summary>
        [JsonPropertyName("ps")]
        public PositionSide PositionSide { get; set; }
        /// <summary>
        /// ["<c>rp</c>"] Profit and loss of the transaction
        /// </summary>
        [JsonPropertyName("rp")]
        public decimal? ProfitAndLoss { get; set; }
        /// <summary>
        /// ["<c>ro</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("ro")]
        public bool? ReduceOnly { get; set; }
    }
}
