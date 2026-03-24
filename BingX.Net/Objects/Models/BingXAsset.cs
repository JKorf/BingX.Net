using BingX.Net.Objects.Internal;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
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
        /// ["<c>name</c>"] Name 
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>coin</c>"] Asset
        /// </summary>
        [JsonPropertyName("coin")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>displayName</c>"] Display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>networkList</c>"] Networks
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
        /// ["<c>name</c>"] Network name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>network</c>"] Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>depositEnable</c>"] Is deposit enabled
        /// </summary>
        [JsonPropertyName("depositEnable")]
        public bool DepositEnabled { get; set; }
        /// <summary>
        /// ["<c>depositMin</c>"] Minimal deposit amount
        /// </summary>
        [JsonPropertyName("depositMin")]
        public decimal MinDeposit { get; set; }
        /// <summary>
        /// ["<c>minConfirm</c>"] Min amount of confirmations
        /// </summary>
        [JsonPropertyName("minConfirm")]
        public int MinConfirmations { get; set; }
        /// <summary>
        /// ["<c>isDefault</c>"] Is default network
        /// </summary>
        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }
        /// <summary>
        /// ["<c>withdrawEnable</c>"] Is withdrawing enabled
        /// </summary>
        [JsonPropertyName("withdrawEnable")]
        public bool WithdrawEnabled { get; set; }
        /// <summary>
        /// ["<c>withdrawFee</c>"] Withdrawal fee
        /// </summary>
        [JsonPropertyName("withdrawFee")]
        public decimal WithdrawFee { get; set; }
        /// <summary>
        /// ["<c>withdrawMin</c>"] Minimal withdrawal
        /// </summary>
        [JsonPropertyName("withdrawMin")]
        public decimal MinWithdraw { get; set; }
        /// <summary>
        /// ["<c>withdrawMax</c>"] Max withdrawal
        /// </summary>
        [JsonPropertyName("withdrawMax")]
        public decimal MaxWithdraw { get; set; }
        /// <summary>
        /// ["<c>withdrawDesc</c>"] Description
        /// </summary>
        [JsonPropertyName("withdrawDesc")]
        public string? WithdrawDescription { get; set; }
        /// <summary>
        /// ["<c>withdrawPrecision</c>"] Witdraw precision
        /// </summary>
        [JsonPropertyName("withdrawPrecision")]
        public int WithdrawPrecision { get; set; }
        /// <summary>
        /// ["<c>depositPrecision</c>"] Deposit precision
        /// </summary>
        [JsonPropertyName("depositPrecision")]
        public int DepositPrecision { get; set; }
        /// <summary>
        /// ["<c>contractAddress</c>"] Contract address
        /// </summary>
        [JsonPropertyName("contractAddress")]
        public string ContractAddress { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>needTagOrMemo</c>"] Needs tag or memo
        /// </summary>
        [JsonPropertyName("needTagOrMemo")]
        public string NeedsTagOrMemo { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>displayName</c>"] Display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
