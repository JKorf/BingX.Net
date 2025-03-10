using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Multi asset mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MultiAssetMode>))]
    public enum MultiAssetMode
    {
        /// <summary>
        /// Single asset mode
        /// </summary>
        [Map("singleAssetMode")]
        SingleAssetMode,
        /// <summary>
        /// Multi asset mode
        /// </summary>
        [Map("multiAssetsMode")]
        MultiAssetMode
    }
}
