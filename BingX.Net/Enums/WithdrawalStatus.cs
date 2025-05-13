using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Withdrawal status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WithdrawalStatus>))]
    public enum WithdrawalStatus
    {
        /// <summary>
        /// Under review
        /// </summary>
        [Map("4")]
        UnderReview,
        /// <summary>
        /// Failed
        /// </summary>
        [Map("5")]
        Failed,
        /// <summary>
        /// Completed
        /// </summary>
        [Map("6")]
        Completed
    }
}
