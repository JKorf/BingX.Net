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
        /// ["<c>sourceAddress</c>"] Source address
        /// </summary>
        [JsonPropertyName("sourceAddress")]
        public string SourceAddress { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>address</c>"] Withdrawal address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>applyTime</c>"] Apply time
        /// </summary>
        [JsonPropertyName("applyTime")]
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// ["<c>id</c>"] Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>withdrawOrderId</c>"] Withdrawal order id
        /// </summary>
        [JsonPropertyName("withdrawOrderId")]
        public string WithdrawOrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>coin</c>"] Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>network</c>"] Withdrawal network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Withdrawal status
        /// </summary>
        [JsonPropertyName("status")]
        public WithdrawalStatus Status { get; set; }
        /// <summary>
        /// ["<c>transferType</c>"] Transfer type
        /// </summary>
        [JsonPropertyName("transferType")]
        public int TransferType { get; set; }
        /// <summary>
        /// ["<c>transactionFee</c>"] Fee
        /// </summary>
        [JsonPropertyName("transactionFee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>confirmNo</c>"] Confirmations
        /// </summary>
        [JsonPropertyName("confirmNo")]
        public int Confirmations { get; set; }
        /// <summary>
        /// ["<c>addressTag</c>"] Address tag
        /// </summary>
        [JsonPropertyName("addressTag")]
        public string? AddressTag { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>info</c>"] Extra info
        /// </summary>
        [JsonPropertyName("info")]
        public string Info { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>txId</c>"] Transaction id
        /// </summary>
        [JsonPropertyName("txId")]
        public string TransactionId { get; set; } = string.Empty;
    }
}
