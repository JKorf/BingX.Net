﻿using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Enums
{
    /// <summary>
    /// API key permission types
    /// </summary>
    public enum ApiKeyPermission
    {
        /// <summary>
        /// Spot trading
        /// </summary>
        [Map("1")]
        SpotTrading,
        /// <summary>
        /// Read
        /// </summary>
        [Map("2")]
        Read,
        /// <summary>
        /// Perpetual futures trading
        /// </summary>
        [Map("3")]
        PerpetualFuturesTrading,
        /// <summary>
        /// Universal transfer
        /// </summary>
        [Map("4")]
        UniversalTransfer,
        /// <summary>
        /// Permission 6
        /// </summary>
        [Map("6")]
        Undocumented6,
        /// <summary>
        /// Internal transfer of sub accounts
        /// </summary>
        [Map("7")]
        InternalTransferOfSubaccounts
    }
}
