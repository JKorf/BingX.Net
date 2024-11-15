using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Objects.Options
{
    /// <summary>
    /// BingX services options
    /// </summary>
    public class BingXOptions
    {
        /// <summary>
        /// Rest client options
        /// </summary>
        public BingXRestOptions Rest { get; set; } = new BingXRestOptions();

        /// <summary>
        /// Socket client options
        /// </summary>
        public BingXSocketOptions Socket { get; set; } = new BingXSocketOptions();

        /// <summary>
        /// Trade environment. Contains info about URL's to use to connect to the API. Use `BingXEnvironment` to swap environment, for example `Environment = BingXEnvironment.Live`
        /// </summary>
        public BingXEnvironment? Environment { get; set; }

        /// <summary>
        /// The api credentials used for signing requests.
        /// </summary>
        public ApiCredentials? ApiCredentials { get; set; }

        /// <summary>
        /// The DI service lifetime for the IBingXSocketClient
        /// </summary>
        public ServiceLifetime? SocketClientLifeTime { get; set; }
    }
}
