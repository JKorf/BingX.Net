using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXQuery : Query<BingXSocketResponse>
    {
        private readonly SocketApiClient _client;

        public BingXQuery(SocketApiClient client, BingXSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            _client = client;
            MessageMatcher = MessageMatcher.Create<BingXSocketResponse>(request.Id, HandleMessage);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<BingXSocketResponse>(request.Id, HandleMessage);
        }

        public CallResult<BingXSocketResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BingXSocketResponse message)
        {
            if (message.Code != 0)
                return new CallResult<BingXSocketResponse>(new ServerError(message.Code.ToString(), _client.GetErrorInfo(message.Code, message.Message!)), originalData);

            return new CallResult<BingXSocketResponse>(message, originalData, null);
        }
    }
}
