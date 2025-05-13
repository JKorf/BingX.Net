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
        /// Funding Account -> Standard Contract
        /// </summary>
        [Map("FUND_SFUTURES")]
        FundingToStandardFutures,
        /// <summary>
        /// Standard Contract -> Funding Account
        /// </summary>
        [Map("SFUTURES_FUND")]
        StandardFuturesToFunding,
        /// <summary>
        /// Funding Account -> Perpetual Futures
        /// </summary>
        [Map("FUND_PFUTURES")]
        FundingToPerpetualFutures,
        /// <summary>
        /// Perpetual Futures -> Funding Account
        /// </summary>
        [Map("PFUTURES_FUND")]
        PerpetualFuturesToFunding,
        /// <summary>
        /// Standard Contract -> Perpetual Futures
        /// </summary>
        [Map("SFUTURES_PFUTURES")]
        StandardFuturesToPerpetualFutures,
        /// <summary>
        /// Perpetual Futures -> Standard Contract
        /// </summary>
        [Map("PFUTURES_SFUTURES")]
        PerpetualFuturesToStandardFutures
    }
}
