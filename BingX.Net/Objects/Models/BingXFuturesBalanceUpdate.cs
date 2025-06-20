using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
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
        /// Account change info
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
        /// Event trigger reason
        /// </summary>
        [JsonPropertyName("m")]
        public string Trigger { get; set; } = string.Empty;

        /// <summary>
        /// Balance changes
        /// </summary>
        [JsonPropertyName("B")]
        public BingXFuturesBalanceChange[] Balances { get; set; } = Array.Empty<BingXFuturesBalanceChange>();

        /// <summary>
        /// Position changes
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
        /// Asset name
        /// </summary>
        [JsonPropertyName("a")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Wallet balance
        /// </summary>
        [JsonPropertyName("wb")]
        public decimal Balance { get; set; }
        /// <summary>
        /// Balance excluding isolated margin
        /// </summary>
        [JsonPropertyName("cw")]
        public decimal BalanceExIsolatedMargin { get; set; }
        /// <summary>
        /// Balance change
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
        /// Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Position size
        /// </summary>
        [JsonPropertyName("pa")]
        public decimal Size { get; set; }
        /// <summary>
        /// Entry price
        /// </summary>
        [JsonPropertyName("ep")]
        public decimal EntryPrice { get; set; }
        /// <summary>
        /// Unrealized profit and loss
        /// </summary>
        [JsonPropertyName("up")]
        public decimal UnrealizedPnl { get; set; }
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("mt")]
        public MarginMode MarginMode { get; set; }
        /// <summary>
        /// Position margin (isolated position)
        /// </summary>
        [JsonPropertyName("iw")]
        public decimal? PositionMargin { get; set; }
        /// <summary>
        /// Position side
        /// </summary>
        [JsonPropertyName("ps")]
        public TradeSide Side { get; set; }
        /// <summary>
        /// Realized profit and loss of positions
        /// </summary>
        [JsonPropertyName("cr")]
        public decimal? RealizedPnl { get; set; }
    }
}
