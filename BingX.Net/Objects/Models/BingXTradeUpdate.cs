using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Trade update info
    /// </summary>
    public record BingXTradeUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonProperty("t")]
        public long TradeId { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonProperty("p")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("q")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Trade timestamp
        /// </summary>
        [JsonProperty("T")]
        public DateTime TradeTime { get; set; }
        /// <summary>
        /// Whether buyer is maker
        /// </summary>
        [JsonProperty("m")]
        public bool BuyerIsMaker { get; set; }
    }
}
