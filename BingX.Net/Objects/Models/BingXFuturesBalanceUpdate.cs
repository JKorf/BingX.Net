using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Enums;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Account update
    /// </summary>
    [SerializationModel]
    public record BingXFuturesAccountUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// ["<c>a</c>"] Account change info
        /// </summary>
        [JsonPropertyName("a")]
        public BingXFuturesAccountChange Update { get; set; } = null!;
    }

    /// <summary>
    /// Account change info
    /// </summary>
    public record BingXFuturesAccountChange
    {
        /// <summary>
        /// ["<c>m</c>"] Event trigger reason
        /// </summary>
        [JsonPropertyName("m")]
        public string Trigger { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>B</c>"] Balance changes
        /// </summary>
        [JsonPropertyName("B")]
        public BingXFuturesBalanceChange[] Balances { get; set; } = Array.Empty<BingXFuturesBalanceChange>();

        /// <summary>
        /// ["<c>P</c>"] Position changes
        /// </summary>
        [JsonPropertyName("P")]
        public BingXFuturesPositionChange[] Positions { get; set; } = Array.Empty<BingXFuturesPositionChange>();
    }

    /// <summary>
    /// Balance change info
    /// </summary>
    [SerializationModel]
    public record BingXFuturesBalanceChange
    {
        /// <summary>
        /// ["<c>a</c>"] Asset name
        /// </summary>
        [JsonPropertyName("a")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>wb</c>"] Wallet balance
        /// </summary>
        [JsonPropertyName("wb")]
        public decimal Balance { get; set; }
        /// <summary>
        /// ["<c>cw</c>"] Balance excluding isolated margin
        /// </summary>
        [JsonPropertyName("cw")]
        public decimal BalanceExIsolatedMargin { get; set; }
        /// <summary>
        /// ["<c>bc</c>"] Balance change
        /// </summary>
        [JsonPropertyName("bc")]
        public decimal BalanceChange { get; set; }
    }

    /// <summary>
    /// Position change info
    /// </summary>
    [SerializationModel]
    public record BingXFuturesPositionChange
    {
        /// <summary>
        /// ["<c>s</c>"] Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pa</c>"] Position size
        /// </summary>
        [JsonPropertyName("pa")]
        public decimal Size { get; set; }
        /// <summary>
        /// ["<c>ep</c>"] Entry price
        /// </summary>
        [JsonPropertyName("ep")]
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// ["<c>up</c>"] Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("up")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// ["<c>mt</c>"] Margin mode
        /// </summary>
        [JsonPropertyName("mt")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// ["<c>iw</c>"] Position margin (isolated position)
        /// </summary>
        [JsonPropertyName("iw")]
        public decimal? PositionMargin { get; set; }
        /// <summary>
        /// ["<c>ps</c>"] Position side
        /// </summary>
        [JsonPropertyName("ps")]
        public TradeSide Side { get; set; }
        /// <summary>
        /// ["<c>cr</c>"] Realized profit and loss of positions
        /// </summary>
        [JsonPropertyName("cr")]
        public decimal? RealizedPnl { get; set; }
    }
}
