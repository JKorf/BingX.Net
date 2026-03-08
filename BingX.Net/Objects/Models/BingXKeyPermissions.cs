using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Text.Json.Serialization;
using BingX.Net.Objects.Internal;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Api key permissions
    /// </summary>
    [SerializationModel(typeof(BingXResult<>))]
    public record BingXKeyPermissions
    {
        /// <summary>
        /// ["<c>apiKey</c>"] Api key
        /// </summary>
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>permissions</c>"] Permissions
        /// </summary>
        [JsonPropertyName("permissions")]
        public ApiKeyPermission[] Permissions { get; set; } = Array.Empty<ApiKeyPermission>();
        /// <summary>
        /// ["<c>ipAddresses</c>"] Ip addresses
        /// </summary>
        [JsonPropertyName("ipAddresses")]
        public string[] IpAddresses { get; set; } = Array.Empty<string>();
        /// <summary>
        /// ["<c>note</c>"] Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
    }


}
