using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Income type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<IncomeType>))]
    public enum IncomeType
    {
        /// <summary>
        /// Transfer
        /// </summary>
        [Map("TRANSFER")]
        Transfer,
        /// <summary>
        /// Realized profit and loss
        /// </summary>
        [Map("REALIZED_PNL")]
        RealizedPnl,
        /// <summary>
        /// Funding Fee
        /// </summary>
        [Map("FUNDING_FEE")]
        FundingFee,
        /// <summary>
        /// Trading Fee
        /// </summary>
        [Map("TRADING_FEE")]
        TradingFee,
        /// <summary>
        /// Liquidation
        /// </summary>
        [Map("INSURANCE_CLEAR")]
        InsuranceClear,
        /// <summary>
        /// Trial Fund
        /// </summary>
        [Map("TRIAL_FUND")]
        TrailFund,
        /// <summary>
        /// Automatic Deleveraging
        /// </summary>
        [Map("ADL")]
        Adl,
        /// <summary>
        /// System deduction
        /// </summary>
        [Map("SYSTEM_DEDUCTION")]
        SystemDeduction,
        /// <summary>
        /// Guaranteed price
        /// </summary>
        [Map("GTD_PRICE")]
        GtdPrice
    }
}
