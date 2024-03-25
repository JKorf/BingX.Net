using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Direction to adjust in
    /// </summary>
    public enum AdjustDirection
    {
        /// <summary>
        /// Increase
        /// </summary>
        [Map("1")]
        Increase,
        /// <summary>
        /// Decrease
        /// </summary>
        [Map("2")]
        Decrease
    }
}
