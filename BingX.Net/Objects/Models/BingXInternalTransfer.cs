using BingX.Net.Enums;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Transfers
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
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
        public BingXInternalTransfer[] Transfers { get; set; } = Array.Empty<BingXInternalTransfer>();
        
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
        /// Quantity transferred
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
        /// <summary>
        /// From
        /// </summary>
        [JsonPropertyName("fromUid")]
        public long FromUid { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("recordType")]
        public TransferDirection TransferType { get; set; }
        /// <summary>
        /// Client id
        /// </summary>
        [JsonPropertyName("transferClientId")]
        public string? TransferClientId { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
    }
}
