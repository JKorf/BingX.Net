using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Margin mode
    /// </summary>
    public enum MarginMode
    {
        /// <summary>
        /// Isolated margin mode
        /// </summary>
        [Map("ISOLATED", "isolated")]
        Isolated,
        /// <summary>
        /// Cross margin mode
        /// </summary>
        [Map("CROSSED", "cross")]
        Cross
    }
}
