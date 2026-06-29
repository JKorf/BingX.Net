using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace BingX.Net.Clients
{
    /// <inheritdoc />
    public class BingXUserClientProvider : UserClientProvider<
        IBingXRestClient,
        IBingXSocketClient,
        BingXRestOptions,
        BingXSocketOptions,
        BingXCredentials,
        BingXEnvironment
        >, IBingXUserClientProvider
    {        
        /// <inheritdoc />
        public override string ExchangeName => BingXExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public BingXUserClientProvider(Action<BingXOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BingXUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<BingXRestOptions> restOptions,
            IOptions<BingXSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
        }

        /// <inheritdoc />
        protected override IBingXRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<BingXRestOptions> options)
            => new BingXRestClient(client, loggerFactory, options);
        /// <inheritdoc />
        protected override IBingXSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<BingXSocketOptions> options)
            => new BingXSocketClient(options, loggerFactory);
    }
}
