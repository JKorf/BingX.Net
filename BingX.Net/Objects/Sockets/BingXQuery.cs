using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using BingX.Net.Objects.Models;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXQuery : Query<BingXSocketResponse>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public BingXQuery(BingXSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ListenerIdentifiers = new HashSet<string> { request.Id };
        }

        public override Task<CallResult<BingXSocketResponse>> HandleMessageAsync(SocketConnection connection, DataEvent<BingXSocketResponse> message)
        {
            if (message.Data.Code != 0)
                return Task.FromResult(new CallResult<BingXSocketResponse>(new ServerError(message.Data.Code, message.Data.Message!), message.OriginalData));

            return Task.FromResult(new CallResult<BingXSocketResponse>(message.Data, message.OriginalData, null));
        }
    }
}
