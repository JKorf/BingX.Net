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
        /// ["<c>TRANSFER</c>"] Transfer
        /// </summary>
        [Map("TRANSFER")]
        Transfer,
        /// <summary>
        /// ["<c>REALIZED_PNL</c>"] Realized profit and loss
        /// </summary>
        [Map("REALIZED_PNL")]
        RealizedPnl,
        /// <summary>
        /// ["<c>FUNDING_FEE</c>"] Funding Fee
        /// </summary>
        [Map("FUNDING_FEE")]
        FundingFee,
        /// <summary>
        /// ["<c>TRADING_FEE</c>"] Trading Fee
        /// </summary>
        [Map("TRADING_FEE")]
        TradingFee,
        /// <summary>
        /// ["<c>INSURANCE_CLEAR</c>"] Liquidation
        /// </summary>
        [Map("INSURANCE_CLEAR")]
        InsuranceClear,
        /// <summary>
        /// ["<c>TRIAL_FUND</c>"] Trial Fund
        /// </summary>
        [Map("TRIAL_FUND")]
        TrailFund,
        /// <summary>
        /// ["<c>ADL</c>"] Automatic Deleveraging
        /// </summary>
        [Map("ADL")]
        Adl,
        /// <summary>
        /// ["<c>SYSTEM_DEDUCTION</c>"] System deduction
        /// </summary>
        [Map("SYSTEM_DEDUCTION")]
        SystemDeduction,
        /// <summary>
        /// ["<c>GTD_PRICE</c>"] Guaranteed price
        /// </summary>
        [Map("GTD_PRICE")]
        GtdPrice
    }
}
