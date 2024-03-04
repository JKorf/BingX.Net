using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using BingX.Net.Objects.Models;

namespace BingX.Net.Objects.Sockets
{
    internal class BingXQuery<T> : Query<T>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public BingXQuery(BingXModel request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ListenerIdentifiers = new HashSet<string> { };
        }

        public override Task<CallResult<T>> HandleMessageAsync(SocketConnection connection, DataEvent<T> message)
        {
            return Task.FromResult(new CallResult<T>(message.Data, message.OriginalData, null));
        }
    }
}
