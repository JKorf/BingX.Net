using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Transfer account type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TransferAccountType>))]
    public enum TransferAccountType
    {
        /// <summary>
        /// Funding account
        /// </summary>
        [Map("fund")]
        Funding,
        /// <summary>
        /// Spot account
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// Standard contract account
        /// </summary>
        [Map("stdFutures")]
        StandardContract,
        /// <summary>
        /// USDT perpetual futures account
        /// </summary>
        [Map("USDTMPerp")]
        UsdtPerpetualFutures,
        /// <summary>
        /// Coin perpetual futures account
        /// </summary>
        [Map("coinMPerp")]
        CoinPerpetualFutures
    }
}
