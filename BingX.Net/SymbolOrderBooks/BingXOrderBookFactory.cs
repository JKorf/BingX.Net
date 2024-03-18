using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BingX.Net.Interfaces;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Options;

namespace BingX.Net.SymbolOrderBooks
{
    /// <summary>
    /// BingX order book factory
    /// </summary>
    public class BingXOrderBookFactory : IBingXOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BingXOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<BingXOrderBookOptions>? options = null)
            => new BingXSpotSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILogger<BingXSpotSymbolOrderBook>>(),
                                             _serviceProvider.GetRequiredService<IBingXRestClient>(),
                                             _serviceProvider.GetRequiredService<IBingXSocketClient>());


        /// <inheritdoc />
        public ISymbolOrderBook CreatePerpetualFutures(string symbol, Action<BingXOrderBookOptions>? options = null)
            => new BingXPerpetualFuturesSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILogger<BingXPerpetualFuturesSymbolOrderBook>>(),
                                             _serviceProvider.GetRequiredService<IBingXRestClient>(),
                                             _serviceProvider.GetRequiredService<IBingXSocketClient>());
    }
}
