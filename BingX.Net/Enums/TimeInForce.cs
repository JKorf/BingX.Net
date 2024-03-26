using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Time in force
    /// </summary>
    public enum TimeInForce
    {
        /// <summary>
        /// Post only order
        /// </summary>
        [Map("PostOnly")]
        PostOnly,
        /// <summary>
        /// Good till canceled
        /// </summary>
        [Map("GTC")]
        GoodTillCanceled,
        /// <summary>
        /// At least partially fill the order upon placement or cancel
        /// </summary>
        [Map("IOC")]
        ImmediateOrCancel,
        /// <summary>
        /// Fill the order upon placement or cancel
        /// </summary>
        [Map("FOK")]
        FillOrKill
    }
}
