using BingX.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BingX.Net.Objects.Models
{
    internal record BingXSymbolsWrapper
    {
        [JsonProperty("symbols")]
        public IEnumerable<BingXSymbol> Symbols { get; set; } = Array.Empty<BingXSymbol>();
    }

    /// <summary>
    /// BingX symbol info
    /// </summary>
    public class BingXSymbol
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonProperty("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Min order quantity
        /// </summary>
        [JsonProperty("minQty")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// Max order quantity
        /// </summary>
        [JsonProperty("maxQty")]
        public decimal MaxOrderQuantity { get; set; }
        /// <summary>
        /// Min notional value of an order
        /// </summary>
        [JsonProperty("minNotional")]
        public decimal MinNotional { get; set; }
        /// <summary>
        /// Max notional value of an order
        /// </summary>
        [JsonProperty("maxNotional")]
        public decimal MaxNotional { get; set; }
        /// <summary>
        /// Symbol status
        /// </summary>
        [JsonProperty("status")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// Tick size
        /// </summary>
        [JsonProperty("tickSize")]
        public decimal TickSize { get; set; }
        /// <summary>
        /// Step size
        /// </summary>
        [JsonProperty("stepSize")]
        public decimal StepSize { get; set; }
        /// <summary>
        /// Is this symbol buyable via the API
        /// </summary>
        [JsonProperty("apiStateSell")]
        public bool ApiBuyable { get; set; }
        /// <summary>
        /// Is this symbol sellable via the API
        /// </summary>
        [JsonProperty("apiStateBuy")]
        public bool ApiSellable { get; set; }
        /// <summary>
        /// Time online
        /// </summary>
        [JsonProperty("timeOnline")]
        public DateTime? TimeOnline { get; set; }
    }
}
