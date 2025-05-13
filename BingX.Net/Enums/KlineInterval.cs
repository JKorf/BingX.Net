using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KlineInterval>))]
    public enum KlineInterval
    {
        /// <summary>
        /// One minute
        /// </summary>
        [Map("1m")]
        OneMinute = 60,
        /// <summary>
        /// Three minutes
        /// </summary>
        [Map("3m")]
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// Five minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// Fifteen minutes
        /// </summary>
        [Map("15m")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// Thirty minutes
        /// </summary>
        [Map("30m")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// One hour
        /// </summary>
        [Map("1h")]
        OneHour = 60 * 60,
        /// <summary>
        /// Two hours
        /// </summary>
        [Map("2h")]
        TwoHours = 60 * 60 * 2,
        /// <summary>
        /// Four hours
        /// </summary>
        [Map("4h")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// Six hours
        /// </summary>
        [Map("6h")]
        SixHours = 60 * 60 * 6,
        /// <summary>
        /// Eight hours
        /// </summary>
        [Map("8h")]
        EightHours = 60 * 60 * 8,
        /// <summary>
        /// Twelve hours
        /// </summary>
        [Map("12h")]
        TwelveHours = 60 * 60 * 12,
        /// <summary>
        /// One day
        /// </summary>
        [Map("1d")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// Three days
        /// </summary>
        [Map("3d")]
        ThreeDay = 60 * 60 * 24 * 3,
        /// <summary>
        /// One week
        /// </summary>
        [Map("1w")]
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// One month
        /// </summary>
        [Map("1M")]
        OneMonth = 60 * 60 * 24 * 30
    }
}
