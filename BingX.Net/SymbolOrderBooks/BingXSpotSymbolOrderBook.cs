using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.Logging;
using BingX.Net.Clients;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Options;
using BingX.Net.Objects.Models;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Linq;
using CryptoExchange.Net.Interfaces;

namespace BingX.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class BingXSpotSymbolOrderBook : SymbolOrderBook
    {
        private readonly bool _clientOwner;
        private readonly IBingXRestClient _restClient;
        private readonly IBingXSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BingXSpotSymbolOrderBook(string symbol, Action<BingXOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="restClient">Rest client instance</param>
        /// <param name="socketClient">Socket client instance</param>
        public BingXSpotSymbolOrderBook(
            string symbol,
            Action<BingXOrderBookOptions>? optionsDelegate,
            ILoggerFactory? logger,
            IBingXRestClient? restClient,
            IBingXSocketClient? socketClient) : base(logger, "BingX", "Spot", symbol)
        {
            var options = BingXOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _strictLevels = false;
            _sequencesAreConsecutive = options?.Limit == null;

            Levels = options?.Limit;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new BingXSocketClient();
            _restClient = restClient ?? new BingXRestClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var result = await _socketClient.SpotApi.SubscribeToPartialOrderBookUpdatesAsync(Symbol, Levels ?? 20, HandleOrderBookUpdate).ConfigureAwait(false);
            if (!result)
                return result;

            if (ct.IsCancellationRequested)
            {
                await result.Data.CloseAsync().ConfigureAwait(false);
                return result.AsError<UpdateSubscription>(new CancellationRequestedError());
            }

            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
            return setResult ? result : new CallResult<UpdateSubscription>(setResult.Error!);
        }

        private void HandleOrderBookUpdate(DataEvent<BingXOrderBook> @event)
        {
            SetInitialOrderBook(DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow)!.Value, @event.Data.Bids, @event.Data.Asks);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
            {
                _restClient?.Dispose();
                _socketClient?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
