using BingX.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    internal record BingXSymbolsWrapper
    {
        [JsonProperty("symbols")]
        [JsonPropertyName("symbols")]
        public IEnumerable<BingXSymbol> Symbols { get; set; } = Array.Empty<BingXSymbol>();
    }

    /// <summary>
    /// BingX symbol info
    /// </summary>
    public record BingXSymbol
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonProperty("symbol")]
        [JsonPropertyName("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonProperty("minQty")]
        [JsonPropertyName("minQty")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// Max order quantity
        /// </summary>
        [JsonProperty("maxQty")]
        [JsonPropertyName("maxQty")]
        public decimal MaxOrderQuantity { get; set; }
        /// <summary>
        /// Min notional value of an order
        /// </summary>
        [JsonProperty("minNotional")]
        [JsonPropertyName("minNotional")]
        public decimal MinNotional { get; set; }
        /// <summary>
        /// Max notional value of an order
        /// </summary>
        [JsonProperty("maxNotional")]
        [JsonPropertyName("maxNotional")]
        public decimal MaxNotional { get; set; }
        /// <summary>
        /// Symbol status
        /// </summary>
        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Tick size
        /// </summary>
        [JsonProperty("tickSize")]
        [JsonPropertyName("tickSize")]
        public decimal TickSize { get; set; }
        /// <summary>
        /// Step size
        /// </summary>
        [JsonProperty("stepSize")]
        [JsonPropertyName("stepSize")]
        public decimal StepSize { get; set; }
        /// <summary>
        /// Is this symbol buyable via the API
        /// </summary>
        [JsonProperty("apiStateSell")]
        [JsonPropertyName("apiStateSell")]
        public bool ApiBuyable { get; set; }
        /// <summary>
        /// Is this symbol sellable via the API
        /// </summary>
        [JsonProperty("apiStateBuy")]
        [JsonPropertyName("apiStateBuy")]
        public bool ApiSellable { get; set; }
        /// <summary>
        /// Time online
        /// </summary>
        [JsonProperty("timeOnline")]
        [JsonPropertyName("timeOnline")]
        public DateTime? TimeOnline { get; set; }
    }
}
