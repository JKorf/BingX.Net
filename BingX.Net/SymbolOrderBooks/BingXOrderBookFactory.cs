using CryptoExchange.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using BingX.Net.Interfaces;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.OrderBook;

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

            Spot = new OrderBookFactory<BingXOrderBookOptions>((symbol, options) => CreateSpot(symbol, options), (baseAsset, quoteAsset, options) => CreateSpot(baseAsset + quoteAsset, options));
            PerpetualFutures = new OrderBookFactory<BingXOrderBookOptions>((symbol, options) => CreatePerpetualFutures(symbol, options), (baseAsset, quoteAsset, options) => CreatePerpetualFutures(baseAsset + quoteAsset, options));
        }

        /// <inheritdoc />
        public IOrderBookFactory<BingXOrderBookOptions> Spot { get; }
        /// <inheritdoc />
        public IOrderBookFactory<BingXOrderBookOptions> PerpetualFutures { get; }

        /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<BingXOrderBookOptions>? options = null)
            => new BingXSpotSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IBingXRestClient>(),
                                             _serviceProvider.GetRequiredService<IBingXSocketClient>());


        /// <inheritdoc />
        public ISymbolOrderBook CreatePerpetualFutures(string symbol, Action<BingXOrderBookOptions>? options = null)
            => new BingXPerpetualFuturesSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IBingXRestClient>(),
                                             _serviceProvider.GetRequiredService<IBingXSocketClient>());
    }
}
