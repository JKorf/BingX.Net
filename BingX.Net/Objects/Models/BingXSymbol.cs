using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Collections.Generic;
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
        /// Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonPropertyName("minQty")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// Max order quantity
        /// </summary>
        [JsonPropertyName("maxQty")]
        public decimal MaxOrderQuantity { get; set; }
        /// <summary>
        /// Min notional value of an order
        /// </summary>
        [JsonPropertyName("minNotional")]
        public decimal MinNotional { get; set; }
        /// <summary>
        /// Max notional value of an order
        /// </summary>
        [JsonPropertyName("maxNotional")]
        public decimal MaxNotional { get; set; }
        /// <summary>
        /// Symbol status
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Tick size
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal TickSize { get; set; }
        /// <summary>
        /// Step size
        /// </summary>
        [JsonPropertyName("stepSize")]
        public decimal StepSize { get; set; }
        /// <summary>
        /// Is this symbol buyable via the API
        /// </summary>
        [JsonPropertyName("apiStateSell")]
        public bool ApiBuyable { get; set; }
        /// <summary>
        /// Is this symbol sellable via the API
        /// </summary>
        [JsonPropertyName("apiStateBuy")]
        public bool ApiSellable { get; set; }
        /// <summary>
        /// Time online
        /// </summary>
        [JsonPropertyName("timeOnline")]
        public DateTime? TimeOnline { get; set; }
        /// <summary>
        /// Time offline
        /// </summary>
        [JsonPropertyName("offTime")]
        public DateTime? TimeOffline { get; set; }
        /// <summary>
        /// Maintenance online
        /// </summary>
        [JsonPropertyName("maintainTime")]
        public DateTime? TimeMaintenance { get; set; }
    }
}
