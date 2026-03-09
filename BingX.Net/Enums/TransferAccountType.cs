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
        /// ["<c>fund</c>"] Funding account
        /// </summary>
        [Map("fund")]
        Funding,
        /// <summary>
        /// ["<c>spot</c>"] Spot account
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// ["<c>stdFutures</c>"] Standard contract account
        /// </summary>
        [Map("stdFutures")]
        StandardContract,
        /// <summary>
        /// ["<c>USDTMPerp</c>"] USDT perpetual futures account
        /// </summary>
        [Map("USDTMPerp")]
        UsdtPerpetualFutures,
        /// <summary>
        /// ["<c>coinMPerp</c>"] Coin perpetual futures account
        /// </summary>
        [Map("coinMPerp")]
        CoinPerpetualFutures
    }
}
