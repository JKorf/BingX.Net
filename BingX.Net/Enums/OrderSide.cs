using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Order side
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        [Map("BUY")]
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        [Map("SELL")]
        Sell
    }
}
