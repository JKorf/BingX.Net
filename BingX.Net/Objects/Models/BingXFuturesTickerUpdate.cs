using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Ticker information over last 24h
    /// </summary>
    [SerializationModel(typeof(BingXUpdate<>))]
    public record BingXFuturesTickerUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// ["<c>s</c>"] Symbol
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>p</c>"] Price change
        /// </summary>
        [JsonPropertyName("p")]
        public decimal PriceChange { get; set; }
        /// <summary>
        /// ["<c>P</c>"] Price change percentage
        /// </summary>
        [JsonPropertyName("P")]
        public decimal PriceChangePercentage { get; set; }
        /// <summary>
        /// ["<c>o</c>"] Open price
        /// </summary>
        [JsonPropertyName("o")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>h</c>"] High price
        /// </summary>
        [JsonPropertyName("h")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>l</c>"] Low price
        /// </summary>
        [JsonPropertyName("l")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>c</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("c")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>L</c>"] Turnover in USDT
        /// </summary>
        [JsonPropertyName("L")]
        public decimal Turnover { get; set; }
        /// <summary>
        /// ["<c>v</c>"] Volume
        /// </summary>
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>q</c>"] Volume in USDT
        /// </summary>
        [JsonPropertyName("q")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// ["<c>O</c>"] Open time
        /// </summary>
        [JsonPropertyName("O")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// ["<c>C</c>"] Close time
        /// </summary>
        [JsonPropertyName("C")]
        public DateTime CloseTime { get; set; }
        /// <summary>
        /// ["<c>B</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("B")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>b</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("b")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>A</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("A")]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>a</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("a")]
        public decimal BestAskQuantity { get; set; }
    }
}
