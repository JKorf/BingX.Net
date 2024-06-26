using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Trade side
    /// </summary>
    public enum TradeSide
    {
        /// <summary>
        /// Long position
        /// </summary>
        [Map("LONG")]
        Long,
        /// <summary>
        /// Short position
        /// </summary>
        [Map("SHORT")]
        Short
    }
}
