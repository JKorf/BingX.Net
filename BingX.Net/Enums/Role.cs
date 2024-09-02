using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Role
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Maker
        /// </summary>
        [Map("maker")]
        Maker,
        /// <summary>
        /// Taker
        /// </summary>
        [Map("taker")]
        Taker
    }
}
