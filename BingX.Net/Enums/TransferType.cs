using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Transfer type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TransferType>))]
    public enum TransferType
    {
        /// <summary>
        /// ["<c>FUND_SFUTURES</c>"] Funding Account -> Standard Contract
        /// </summary>
        [Map("FUND_SFUTURES")]
        FundingToStandardFutures,
        /// <summary>
        /// ["<c>SFUTURES_FUND</c>"] Standard Contract -> Funding Account
        /// </summary>
        [Map("SFUTURES_FUND")]
        StandardFuturesToFunding,
        /// <summary>
        /// ["<c>FUND_PFUTURES</c>"] Funding Account -> Perpetual Futures
        /// </summary>
        [Map("FUND_PFUTURES")]
        FundingToPerpetualFutures,
        /// <summary>
        /// ["<c>PFUTURES_FUND</c>"] Perpetual Futures -> Funding Account
        /// </summary>
        [Map("PFUTURES_FUND")]
        PerpetualFuturesToFunding,
        /// <summary>
        /// ["<c>SFUTURES_PFUTURES</c>"] Standard Contract -> Perpetual Futures
        /// </summary>
        [Map("SFUTURES_PFUTURES")]
        StandardFuturesToPerpetualFutures,
        /// <summary>
        /// ["<c>PFUTURES_SFUTURES</c>"] Perpetual Futures -> Standard Contract
        /// </summary>
        [Map("PFUTURES_SFUTURES")]
        PerpetualFuturesToStandardFutures
    }
}
