using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Enums;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Withdrawal info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXWithdrawal
    {
        /// <summary>
        /// Withdrawal address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Apply time
        /// </summary>
        [JsonPropertyName("applyTime")]
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Withdrawal network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Withdrawal status
        /// </summary>
        [JsonPropertyName("status")]
        public WithdrawalStatus Status { get; set; }
        /// <summary>
        /// Transfer type
        /// </summary>
        [JsonPropertyName("transferType")]
        public int transferType { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("transactionFee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Confirmations
        /// </summary>
        [JsonPropertyName("confirmNo")]
        public int Confirmations { get; set; }
        /// <summary>
        /// Address tag
        /// </summary>
        [JsonPropertyName("addressTag")]
        public string? AddressTag { get; set; } = string.Empty;
        /// <summary>
        /// Extra info
        /// </summary>
        [JsonPropertyName("info")]
        public string Info { get; set; } = string.Empty;
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("txId")]
        public string TransactionId { get; set; } = string.Empty;
    }
}
