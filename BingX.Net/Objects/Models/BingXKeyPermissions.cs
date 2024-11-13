using BingX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Api key permissions
    /// </summary>
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
        public IEnumerable<ApiKeyPermission> Permissions { get; set; } = Array.Empty<ApiKeyPermission>();
        /// <summary>
        /// Ip addresses
        /// </summary>
        [JsonPropertyName("ipAddresses")]
        public IEnumerable<string> IpAddresses { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Note
        /// </summary>
        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;
    }


}
