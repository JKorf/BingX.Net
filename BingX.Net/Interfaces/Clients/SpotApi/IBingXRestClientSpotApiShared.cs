﻿using CryptoExchange.Net.SharedApis.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingX.Net.Interfaces.Clients.SpotApi
{
    public interface IBingXRestClientSpotApiShared:
        ITickerRestClient,
        ISpotSymbolRestClient,
        IKlineRestClient,
        IRecentTradeRestClient,
        IBalanceRestClient,
        ISpotOrderRestClient
    {
    }
}
