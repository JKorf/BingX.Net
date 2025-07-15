using BingX.Net.Enums;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Deposit addresses
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXDepositAddresses
    {
        /// <summary>
        /// Total
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// Results
        /// </summary>
        [JsonPropertyName("data")]
        public BingXDepositAddress[] Data { get; set; } = Array.Empty<BingXDepositAddress>();
    }

    /// <summary>
    /// Deposit address
    /// </summary>
    public record BingXDepositAddress
    {
        /// <summary>
        /// Asset id
        /// </summary>
        [JsonPropertyName("coinId")]
        public int AssetId { get; set; }
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// Wallet type
        /// </summary>
        [JsonPropertyName("walletType")]
        public AccountType WalletType { get; set; }
    }
}
