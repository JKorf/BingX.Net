using System.Text.Json.Serialization;

namespace BingX.Net.Objects.Models
{
    /// <summary>
    /// Listenkey has expired
    /// </summary>
    public record BingXListenKeyExpiredUpdate : BingXSocketUpdate
    {
        /// <summary>
        /// The listen key
        /// </summary>
        [JsonPropertyName("listenKey")]
        public string ListenKey { get; set; } = string.Empty;
    }
}
