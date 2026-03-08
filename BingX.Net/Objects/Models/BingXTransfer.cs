using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Transfers
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXTransfers
    {
        /// <summary>
        /// ["<c>total</c>"] Total transfers
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// ["<c>rows</c>"] Transfer list
        /// </summary>
        [JsonPropertyName("rows")]
        public BingXTransfer[] Transfers { get; set; } = Array.Empty<BingXTransfer>();
        
    }

    /// <summary>
    /// Transfer info
    /// </summary>
    [SerializationModel]
    public record BingXTransfer
    {
        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Quantity transfered
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Transfer type
        /// </summary>
        [JsonPropertyName("type")]
        public TransferType Type { get; set; }
        /// <summary>
        /// ["<c>tranId</c>"] Transaction id
        /// </summary>
        [JsonPropertyName("tranId")]
        public long TransactionId { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
