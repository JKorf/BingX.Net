using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Contract type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ContractType>))]
    public enum ContractType
    {
        /// <summary>
        /// ["<c>nonCryptoForex</c>"] Forex
        /// </summary>
        [Map("nonCryptoForex")]
        Forex,
        /// <summary>
        /// ["<c>nonCryptoCommodity</c>"] Commodity
        /// </summary>
        [Map("nonCryptoCommodity")]
        Commodity,
        /// <summary>
        /// ["<c>nonCryptoStock</c>"] Stock
        /// </summary>
        [Map("nonCryptoStock")]
        Stock,
        /// <summary>
        /// ["<c>nonCryptoStockIndex</c>"] Stock Index
        /// </summary>
        [Map("nonCryptoStockIndex")]
        StockIndex,
    }
}
