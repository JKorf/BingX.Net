using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using BingX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.PerpetualFuturesApi
{
    internal partial class BingXSocketClientPerpetualFuturesSharedApi :
        SharedApiBase,
        IBingXSocketClientPerpetualFuturesApiShared,
        IBingXSocketClientPerpetualFuturesSharedApi
    {
        private readonly BingXSocketClientPerpetualFuturesApi _api;

        private const string _exchangeName = "BingX";
        private const string _topicId = "BingXPerpFutures";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BingXExchange.Metadata, this);

        public BingXSocketClientPerpetualFuturesSharedApi(BingXSocketClientPerpetualFuturesApi api)
            : base(
                  SharedTransport.Socket,
                  api.Exchange,
                  new[] { TradingMode.PerpetualLinear },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeKlineOptions,
                SubscribeBookTickerOptions,
                SubscribeBalanceOptions,
                SubscribeFuturesOrderOptions,
                SubscribePositionOptions
                );
        }
    }
}
