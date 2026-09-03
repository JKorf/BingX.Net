using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BingX.Net.Clients.SpotApi
{
    internal partial class BingXSocketClientSpotSharedApi : 
        SharedApiBase,
        IBingXSocketClientSpotApiShared,
        IBingXSocketClientSpotSharedApi
    {
        private readonly BingXSocketClientSpotApi _api;

        private const string _exchangeName = "BingX";
        private const string _topicId = "BingXSpot";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BingXExchange.Metadata, this);

        public BingXSocketClientSpotSharedApi(BingXSocketClientSpotApi api)
            : base(
                  SharedTransport.Socket,
                  api.Exchange,
                  new[] { TradingMode.Spot },
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
                SubscribeSpotOrderOptions
                );
        }
    }
}
