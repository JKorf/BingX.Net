using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXSymbolsWrapper
    {
        [JsonPropertyName("symbols")]
        public BingXSymbol[] Symbols { get; set; } = Array.Empty<BingXSymbol>();
    }

    /// <summary>
    /// BingX symbol info
    /// </summary>
    public record BingXSymbol
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>displayName</c>"] Display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>minQty</c>"] Min order quantity
        /// </summary>
        [JsonPropertyName("minQty")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>maxQty</c>"] Max order quantity
        /// </summary>
        [JsonPropertyName("maxQty")]
        public decimal MaxOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>minNotional</c>"] Min notional value of an order
        /// </summary>
        [JsonPropertyName("minNotional")]
        public decimal MinNotional { get; set; }
        /// <summary>
        /// ["<c>maxNotional</c>"] Max notional value of an order
        /// </summary>
        [JsonPropertyName("maxNotional")]
        public decimal MaxNotional { get; set; }
        /// <summary>
        /// ["<c>maxMarketNotional</c>"] Max notional value of a market order
        /// </summary>
        [JsonPropertyName("maxMarketNotional")]
        public decimal MaxMarketNotional { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Symbol status
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>tickSize</c>"] Tick size
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal TickSize { get; set; }
        /// <summary>
        /// ["<c>stepSize</c>"] Step size
        /// </summary>
        [JsonPropertyName("stepSize")]
        public decimal StepSize { get; set; }
        /// <summary>
        /// ["<c>apiStateSell</c>"] Is this symbol buyable via the API
        /// </summary>
        [JsonPropertyName("apiStateSell")]
        public bool ApiBuyable { get; set; }
        /// <summary>
        /// ["<c>apiStateBuy</c>"] Is this symbol sellable via the API
        /// </summary>
        [JsonPropertyName("apiStateBuy")]
        public bool ApiSellable { get; set; }
        /// <summary>
        /// ["<c>timeOnline</c>"] Time online
        /// </summary>
        [JsonPropertyName("timeOnline")]
        public DateTime? TimeOnline { get; set; }
        /// <summary>
        /// ["<c>offTime</c>"] Time offline
        /// </summary>
        [JsonPropertyName("offTime")]
        public DateTime? TimeOffline { get; set; }
        /// <summary>
        /// ["<c>maintainTime</c>"] Maintenance online
        /// </summary>
        [JsonPropertyName("maintainTime")]
        public DateTime? TimeMaintenance { get; set; }
    }
}
