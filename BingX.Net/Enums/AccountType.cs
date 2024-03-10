using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Account type
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Funding account
        /// </summary>
        [Map("1")]
        Funding,
        /// <summary>
        /// Standard account
        /// </summary>
        [Map("2")]
        Standard,
        /// <summary>
        /// Perpetual account
        /// </summary>
        [Map("3")]
        Perpetual
    }
}
