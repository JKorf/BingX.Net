using BingX.Net.Objects.Internal;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BingX.Net.Objects.Sockets.Subscriptions
{
    internal class BingXPingSubscription : SystemSubscription<BingXPing>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string> { "ping" };

        public BingXPingSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override Task<CallResult> HandleMessageAsync(SocketConnection connection, DataEvent<BingXPing> message)
        {
            connection.Send(ExchangeHelpers.NextId(), new BingXPong { Pong = message.Data.Ping, Timestamp = message.Data.Timestamp }, 1);
            return Task.FromResult(new CallResult(null));
        }
    }
}
