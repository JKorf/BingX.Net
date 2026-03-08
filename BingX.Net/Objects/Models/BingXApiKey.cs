using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXApiKeyWrapper
    {
        [JsonPropertyName("apiInfos")]
        public BingXApiKey[] ApiInfos { get; set; } = Array.Empty<BingXApiKey>();
    }

    /// <summary>
    /// API key info
    /// </summary>
    public record BingXApiKey
    {
        /// <summary>
        /// ["<c>apiKey</c>"] The api key
        /// </summary>
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>note</c>"] Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ipAddresses</c>"] IP address restrictions
        /// </summary>
        [JsonPropertyName("ipAddresses")]
        public string[] IpAddresses { get; set; } = Array.Empty<string>();
        /// <summary>
        /// ["<c>permissions</c>"] IP address restrictions
        /// </summary>
        [JsonPropertyName("permissions")]
        public ApiKeyPermission[] Permissions { get; set; } = Array.Empty<ApiKeyPermission>();

        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; }
        /// <summary>
        /// ["<c>createTime</c>"] Create time
        /// </summary>
        [JsonPropertyName("createTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// ["<c>updateTime</c>"] Update time
        /// </summary>
        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }
    }
}
