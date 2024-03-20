using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Position side
    /// </summary>
    public enum PositionSide
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
        Short,
        /// <summary>
        /// Both
        /// </summary>
        [Map("BOTH")]
        Both
    }
}
