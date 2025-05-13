using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;
using BingX.Net.Enums;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Account configuration change update
    /// </summary>
    [SerializationModel]
    public record BingXConfigUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// Configuration info
        /// </summary>
        [JsonPropertyName("ac")]
        public BingXAssetConfig Configuration { get; set; } = null!;
    }

    /// <summary>
    /// Configuration info
    /// </summary>
    public record BingXAssetConfig
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Long position leverage
        /// </summary>
        [JsonPropertyName("l")]
        public decimal LeverageLong { get; set; }
        /// <summary>
        /// Short position leverage
        /// </summary>
        [JsonPropertyName("S")]
        public decimal LeverageShort { get; set; }
        /// <summary>
        /// Margin mode
        /// </summary>
        [JsonPropertyName("mt")]
        public MarginMode MarginMode { get; set; }
    }
}
