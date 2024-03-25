using BingX.Net.Clients;
using BingX.Net.Interfaces.Clients;

namespace CryptoExchange.Net.Interfaces
{
    /// <summary>
    /// Extensions for the ICryptoRestClient and ICryptoSocketClient interfaces
    /// </summary>
    public static class CryptoClientExtensions
    {
        /// <summary>
        /// Get the BingX REST Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IBingXRestClient BingX(this ICryptoRestClient baseClient) => baseClient.TryGet<IBingXRestClient>(() => new BingXRestClient());

        /// <summary>
        /// Get the BingX Websocket Api client
        /// </summary>
        /// <param name="baseClient"></param>
        /// <returns></returns>
        public static IBingXSocketClient BingX(this ICryptoSocketClient baseClient) => baseClient.TryGet<IBingXSocketClient>(() => new BingXSocketClient());
    }
}
