using BingX.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Transfers
    /// </summary>
    public record BingXInternalTransfers
    {
        /// <summary>
        /// Total transfers
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// Transfer list
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<BingXInternalTransfer> Transfers { get; set; } = Array.Empty<BingXInternalTransfer>();
        
    }

    /// <summary>
    /// Transfer info
    /// </summary>
    public record BingXInternalTransfer
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Quantity transfered
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("id")]
        public long TransactionId { get; set; }
        /// <summary>
        /// Receiver
        /// </summary>
        [JsonPropertyName("receiver")]
        public long ReceiverUid { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public InternalTransferStatus Status { get; set; }
    }
}
