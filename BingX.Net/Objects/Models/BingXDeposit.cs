using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Enums;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Deposit info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXDeposit
    {
        /// <summary>
        /// Quantity 
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Used network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Current status
        /// </summary>
        [JsonPropertyName("status")]
        public DepositStatus Status { get; set; }
        /// <summary>
        /// Deposit address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Address tag
        /// </summary>
        [JsonPropertyName("addressTag")]
        public string? AddressTag { get; set; } = string.Empty;
        /// <summary>
        /// Source address
        /// </summary>
        [JsonPropertyName("sourceAddress")]
        public string SourceAddress { get; set; } = string.Empty;
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("txId")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Insert time
        /// </summary>
        [JsonPropertyName("insertTime")]
        public DateTime InsertTime { get; set; }
        /// <summary>
        /// Confirmed times for unlocking
        /// </summary>
        [JsonPropertyName("unlockConfirm")]
        public string UnlockConfirmations { get; set; } = string.Empty;
        /// <summary>
        /// Network confirmations
        /// </summary>
        [JsonPropertyName("confirmTimes")]
        public string ConfirmedTimes { get; set; } = string.Empty;
    }
}
