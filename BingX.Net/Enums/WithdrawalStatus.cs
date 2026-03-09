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
        /// ["<c>4</c>"] Under review
        /// </summary>
        [Map("4")]
        UnderReview,
        /// <summary>
        /// ["<c>5</c>"] Failed
        /// </summary>
        [Map("5")]
        Failed,
        /// <summary>
        /// ["<c>6</c>"] Completed
        /// </summary>
        [Map("6")]
        Completed
    }
}
