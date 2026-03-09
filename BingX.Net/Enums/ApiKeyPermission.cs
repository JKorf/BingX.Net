using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// API key permission types
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ApiKeyPermission>))]
    public enum ApiKeyPermission
    {
        /// <summary>
        /// ["<c>1</c>"] Spot trading
        /// </summary>
        [Map("1")]
        SpotTrading,
        /// <summary>
        /// ["<c>2</c>"] Read
        /// </summary>
        [Map("2")]
        Read,
        /// <summary>
        /// ["<c>3</c>"] Perpetual futures trading
        /// </summary>
        [Map("3")]
        PerpetualFuturesTrading,
        /// <summary>
        /// ["<c>4</c>"] Universal transfer
        /// </summary>
        [Map("4")]
        UniversalTransfer,
        /// <summary>
        /// ["<c>6</c>"] Permission 6
        /// </summary>
        [Map("6")]
        Undocumented6,
        /// <summary>
        /// ["<c>7</c>"] Internal transfer of sub accounts
        /// </summary>
        [Map("7")]
        InternalTransferOfSubaccounts
    }
}
