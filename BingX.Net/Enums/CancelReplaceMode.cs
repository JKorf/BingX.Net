using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Cancel replace mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<CancelReplaceMode>))]
    public enum CancelReplaceMode
    {
        /// <summary>
        /// ["<c>STOP_ON_FAILURE</c>"] Don't place the new order if the cancel failed
        /// </summary>
        [Map("STOP_ON_FAILURE")]
        StopOnFailure,
        /// <summary>
        /// ["<c>ALLOW_FAILURE</c>"] Place the new order regardless of the cancel result
        /// </summary>
        [Map("ALLOW_FAILURE")]
        AllowFailure,
    }
}
