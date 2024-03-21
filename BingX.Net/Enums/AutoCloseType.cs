using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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
