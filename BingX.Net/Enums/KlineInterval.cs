using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    public enum KlineInterval
    {
        /// <summary>
        /// One minute
        /// </summary>
        [Map("1m")]
        OneMinute,
        /// <summary>
        /// Three minutes
        /// </summary>
        [Map("3m")]
        ThreeMinutes,
        /// <summary>
        /// Five minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes,
        /// <summary>
        /// Fifteen minutes
        /// </summary>
        [Map("15m")]
        FifteenMinutes,
        /// <summary>
        /// Thirty minutes
        /// </summary>
        [Map("30m")]
        ThirtyMinutes,
        /// <summary>
        /// One hour
        /// </summary>
        [Map("1h")]
        OneHour,
        /// <summary>
        /// Two hours
        /// </summary>
        [Map("2h")]
        TwoHours,
        /// <summary>
        /// Four hours
        /// </summary>
        [Map("4h")]
        FourHours,
        /// <summary>
        /// Six hours
        /// </summary>
        [Map("6h")]
        SixHours,
        /// <summary>
        /// Eight hours
        /// </summary>
        [Map("8h")]
        EightHours,
        /// <summary>
        /// Twelve hours
        /// </summary>
        [Map("12h")]
        TwelveHours,
        /// <summary>
        /// One day
        /// </summary>
        [Map("1d")]
        OneDay,
        /// <summary>
        /// Three days
        /// </summary>
        [Map("3d")]
        ThreeDay,
        /// <summary>
        /// One week
        /// </summary>
        [Map("1w")]
        OneWeek,
        /// <summary>
        /// One month
        /// </summary>
        [Map("1M")]
        OneMonth
    }
}
