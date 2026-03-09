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
        /// ["<c>0</c>"] In progress
        /// </summary>
        [Map("0")]
        InProgress,
        /// <summary>
        /// ["<c>6</c>"] Chain uploaded
        /// </summary>
        [Map("6")]
        ChainUploaded,
        /// <summary>
        /// ["<c>1</c>"] Completed
        /// </summary>
        [Map("1")]
        Completed
    }
}
