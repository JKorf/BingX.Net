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
        /// ["<c>total</c>"] Total transfers
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// ["<c>data</c>"] Transfer list
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
        /// ["<c>coin</c>"] Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Quantity transferred
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Transaction id
        /// </summary>
        [JsonPropertyName("id")]
        public long TransactionId { get; set; }
        /// <summary>
        /// ["<c>receiver</c>"] Receiver
        /// </summary>
        [JsonPropertyName("receiver")]
        public long ReceiverUid { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public InternalTransferStatus Status { get; set; }
        /// <summary>
        /// ["<c>fromUid</c>"] From
        /// </summary>
        [JsonPropertyName("fromUid")]
        public long FromUid { get; set; }
        /// <summary>
        /// ["<c>recordType</c>"] Type
        /// </summary>
        [JsonPropertyName("recordType")]
        public TransferDirection TransferType { get; set; }
        /// <summary>
        /// ["<c>transferClientId</c>"] Client id
        /// </summary>
        [JsonPropertyName("transferClientId")]
        public string? TransferClientId { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
    }
}
