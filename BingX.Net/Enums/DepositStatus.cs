using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Deposit status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<DepositStatus>))]
    public enum DepositStatus
    {
        /// <summary>
        /// In progress
        /// </summary>
        [Map("0")]
        InProgress,
        /// <summary>
        /// Chain uploaded
        /// </summary>
        [Map("6")]
        ChainUploaded,
        /// <summary>
        /// Completed
        /// </summary>
        [Map("1")]
        Completed
    }
}
