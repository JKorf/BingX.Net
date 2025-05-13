using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    [SerializationModel(typeof(BingXResult<>))]
    internal record BingXListenKey
    {
        [JsonPropertyName("listenKey")]
        public string ListenKey { get; set; } = string.Empty;
    }
}
