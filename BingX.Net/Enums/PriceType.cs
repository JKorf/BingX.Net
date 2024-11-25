using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Price type
    /// </summary>
    public enum PriceType
    {
        /// <summary>
        /// Constant
        /// </summary>
        [Map("constant")]
        Constant,
        /// <summary>
        /// Percentage
        /// </summary>
        [Map("percentage")]
        Percentage
    }
}
