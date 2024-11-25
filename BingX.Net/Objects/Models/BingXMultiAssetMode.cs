using BingX.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Multi asset mode
    /// </summary>
    public record BingXMultiAssetMode
    {
        /// <summary>
        /// Asset mode
        /// </summary>
        [JsonPropertyName("assetMode")]
        public MultiAssetMode AssetMode { get; set; }
    }


}
