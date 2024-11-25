using CryptoExchange.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Multi asset mode
    /// </summary>
    public enum MultiAssetMode
    {
        /// <summary>
        /// Single asset mode
        /// </summary>
        [Map("singleAssetMode")]
        SingleAssetMode,
        /// <summary>
        /// Multi asset mode
        /// </summary>
        [Map("multiAssetsMode")]
        MultiAssetMode
    }
}
