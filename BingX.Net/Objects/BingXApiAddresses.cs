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
        /// The address used by the BingXSocketClient for the Spot websocket API
        /// </summary>
        public string SocketClientSpotAddress { get; set; } = "";
        /// <summary>
        /// The address used by the BingXSocketClient for the Swap websocket API
        /// </summary>
        public string SocketClientSwapAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the BingX.com API
        /// </summary>
        public static BingXApiAddresses Default = new BingXApiAddresses
        {
            RestClientAddress = "https://open-api.bingx.com",
            SocketClientSpotAddress = "wss://open-api-ws.bingx.com",
            SocketClientSwapAddress = "wss://open-api-swap.bingx.com"
        };

        /// <summary>
        /// The addresses to connect to the demo trading BingX.com API. Note that this only applies to trading VST
        /// </summary>
        public static BingXApiAddresses Demo = new BingXApiAddresses
        {
            RestClientAddress = "https://open-api-vst.bingx.com",
            SocketClientSpotAddress = "wss://open-api-ws.bingx.com",
            SocketClientSwapAddress = "wss://vst-open-api-ws.bingx.com"
        };
    }
}
