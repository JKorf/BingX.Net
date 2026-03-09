using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Time in force
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TimeInForce>))]
    public enum TimeInForce
    {
        /// <summary>
        /// ["<c>PostOnly</c>"] Post only order
        /// </summary>
        [Map("PostOnly")]
        PostOnly,
        /// <summary>
        /// ["<c>GTC</c>"] Good till canceled
        /// </summary>
        [Map("GTC")]
        GoodTillCanceled,
        /// <summary>
        /// ["<c>IOC</c>"] At least partially fill the order upon placement or cancel
        /// </summary>
        [Map("IOC")]
        ImmediateOrCancel,
        /// <summary>
        /// ["<c>FOK</c>"] Fill the order upon placement or cancel
        /// </summary>
        [Map("FOK")]
        FillOrKill
    }
}
