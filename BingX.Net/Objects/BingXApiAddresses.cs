namespace BingX.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class BingXApiAddresses
    {
        /// <summary>
        /// The address used by the BingXRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the BingXSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the BingX.com API
        /// </summary>
        public static BingXApiAddresses Default = new BingXApiAddresses
        {
            RestClientAddress = "https://open-api.bingx.com",
            SocketClientAddress = "wss://open-api-ws.bingx.com"
        };
    }
}
