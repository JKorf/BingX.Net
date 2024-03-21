using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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
