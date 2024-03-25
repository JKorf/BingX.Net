using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Position mode
    /// </summary>
    public enum PositionMode
    {
        /// <summary>
        /// Dual position mode
        /// </summary>
        [Map("true")]
        DualPositionMode,
        /// <summary>
        /// Single position mode
        /// </summary>
        [Map("false")]
        SinglePositionMode
    }
}
