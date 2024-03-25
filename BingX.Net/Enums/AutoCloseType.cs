using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Auto closed order type
    /// </summary>
    public enum AutoCloseType
    {
        /// <summary>
        /// Liquidation
        /// </summary>
        [Map("LIQUIDATION")]
        Liquidation,
        /// <summary>
        /// Adl
        /// </summary>
        [Map("ADL")]
        Adl
    }
}
