using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Internal
{
    [SerializationModel]
    internal record BingXUpdate<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("data")]
        public T? Data { get; set; }
        [JsonPropertyName("dataType")]
        public string DataType { get; set; } = string.Empty;
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("s")]
        public string? Symbol { get; set; }
    }
}
