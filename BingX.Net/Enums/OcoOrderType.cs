using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Order type
    /// </summary>
    public enum OcoOrderType
    {
        /// <summary>
        /// Oco limit order
        /// </summary>
        [Map("ocoLimit")]
        OcoLimit,
        /// <summary>
        /// Oco stop limit order
        /// </summary>
        [Map("ocoTps")]
        OcoStopLimit
    }
}
