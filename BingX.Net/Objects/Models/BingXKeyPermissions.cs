using CryptoExchange.Net.Converters.SystemTextJson;
using BingX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
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
        /// Api key
        /// </summary>
        [JsonPropertyName("apiKey")]
        public string ApiKey { get; set; } = string.Empty;
        /// <summary>
        /// Permissions
        /// </summary>
        [JsonPropertyName("permissions")]
        public ApiKeyPermission[] Permissions { get; set; } = Array.Empty<ApiKeyPermission>();
        /// <summary>
        /// Ip addresses
        /// </summary>
        [JsonPropertyName("ipAddresses")]
        public string[] IpAddresses { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
    }


}
