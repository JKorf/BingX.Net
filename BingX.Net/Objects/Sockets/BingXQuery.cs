﻿using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXQuery : Query<BingXSocketResponse>
    {
        public BingXQuery(BingXSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            MessageMatcher = MessageMatcher.Create<BingXSocketResponse>(request.Id, HandleMessage);
        }

        public CallResult<BingXSocketResponse> HandleMessage(SocketConnection connection, DataEvent<BingXSocketResponse> message)
        {
            if (message.Data.Code != 0)
                return new CallResult<BingXSocketResponse>(new ServerError(message.Data.Code, message.Data.Message!), message.OriginalData);

            return new CallResult<BingXSocketResponse>(message.Data, message.OriginalData, null);
        }
    }
}
