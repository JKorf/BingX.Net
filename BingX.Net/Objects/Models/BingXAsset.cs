using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Asset info
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXAsset
    {
        /// <summary>
        /// Name 
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// Networks
        /// </summary>
        [JsonPropertyName("networkList")]
        public BingXNetwork[] Networks { get; set; } = Array.Empty<BingXNetwork>();
    }

    /// <summary>
    /// Network info
    /// </summary>
    [SerializationModel]
    public record BingXNetwork
    {
        /// <summary>
        /// Network name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Is deposit enabled
        /// </summary>
        [JsonPropertyName("depositEnable")]
        public bool DepositEnabled { get; set; }
        /// <summary>
        /// Minimal deposit amount
        /// </summary>
        [JsonPropertyName("depositMin")]
        public decimal MinDeposit { get; set; }
        /// <summary>
        /// Min amount of confirmations
        /// </summary>
        [JsonPropertyName("minConfirm")]
        public int MinConfirmations { get; set; }
        /// <summary>
        /// Is default network
        /// </summary>
        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }
        /// <summary>
        /// Is withdrawing enabled
        /// </summary>
        [JsonPropertyName("withdrawEnable")]
        public bool WithdrawEnabled { get; set; }
        /// <summary>
        /// Withdrawal fee
        /// </summary>
        [JsonPropertyName("withdrawFee")]
        public decimal WithdrawFee { get; set; }
        /// <summary>
        /// Minimal withdrawal
        /// </summary>
        [JsonPropertyName("withdrawMin")]
        public decimal MinWithdraw { get; set; }
        /// <summary>
        /// Max withdrawal
        /// </summary>
        [JsonPropertyName("withdrawMax")]
        public decimal MaxWithdraw { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("withdrawDesc")]
        public string? WithdrawDescription { get; set; }
        /// <summary>
        /// Witdraw precision
        /// </summary>
        [JsonPropertyName("withdrawPrecision")]
        public int WithdrawPrecision { get; set; }
        /// <summary>
        /// Deposit precision
        /// </summary>
        [JsonPropertyName("depositPrecision")]
        public int DepositPrecision { get; set; }
        /// <summary>
        /// Contract address
        /// </summary>
        [JsonPropertyName("contractAddress")]
        public string ContractAddress { get; set; } = string.Empty;
    }
}
