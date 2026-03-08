using BingX.Net.Enums;
using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>total</c>"] Total
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
        /// <summary>
        /// ["<c>data</c>"] Results
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
        /// ["<c>coinId</c>"] Asset id
        /// </summary>
        [JsonPropertyName("coinId")]
        public int AssetId { get; set; }
        /// <summary>
        /// ["<c>coin</c>"] Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>network</c>"] Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>address</c>"] Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tag</c>"] Tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// ["<c>walletType</c>"] Wallet type
        /// </summary>
        [JsonPropertyName("walletType")]
        public AccountType WalletType { get; set; }
    }
}
