using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Ticker information over last 24h
    /// </summary>
    public record BingXTickerUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Price change
        /// </summary>
        [JsonProperty("p")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// Price change percentage
        /// </summary>
        [JsonProperty("P")]
        public string PriceChangePercentage { get; set; } = string.Empty;
        /// <summary>
        /// Open price
        /// </summary>
        [JsonProperty("o")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonProperty("h")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonProperty("l")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonProperty("c")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        [JsonProperty("v")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Quote volume
        /// </summary>
        [JsonProperty("q")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Open time
        /// </summary>
        [JsonProperty("O")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Close time
        /// </summary>
        [JsonProperty("C")]
        public DateTime CloseTime { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonProperty("B")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best bid quantity
        /// </summary>
        [JsonProperty("b")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonProperty("A")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Best ask quantity
        /// </summary>
        [JsonProperty("a")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// Trade count
        /// </summary>
        [JsonProperty("n")]
        public int TradeCount { get; set; }
    }
}
