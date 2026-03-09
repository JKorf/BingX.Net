using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace BingX.Net.Enums
{
    /// <summary>
    /// Status of a symbol
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// ["<c>0</c>"] Offline
        /// </summary>
        [Map("0")]
        Offline,
        /// <summary>
        /// ["<c>1</c>"] Online
        /// </summary>
        [Map("1")]
        Online,
        /// <summary>
        /// ["<c>5</c>"] Pre-open
        /// </summary>
        [Map("5")]
        PreOpen,
        /// <summary>
        /// ["<c>25</c>"] Trading suspended
        /// </summary>
        [Map("25")]
        Suspended,
        /// <summary>
        /// ["<c>29</c>"] Pre-delisted
        /// </summary>
        [Map("29")]
        PreDelisted,
        /// <summary>
        /// ["<c>30</c>"] Delisted
        /// </summary>
        [Map("30")]
        Delisted
    }
}
