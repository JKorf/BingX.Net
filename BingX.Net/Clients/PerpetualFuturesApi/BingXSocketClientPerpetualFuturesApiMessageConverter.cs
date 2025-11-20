using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Net.WebSockets;
using System.Text.Json;

namespace BingX.Net.Clients.SpotApi
{
    internal class BingXSocketClientPerpetualFuturesApiMessageConverter : DynamicJsonConverter
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BingXExchange._serializerContext);

        protected override MessageEvaluator[] MessageEvaluators { get; } = [

            new MessageEvaluator {
                Priority = 1,
                Fields = [
                    new PropertyFieldReference("dataType"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("dataType")
            },

            new MessageEvaluator {
                Priority = 2,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("id")
            }
        ];

        public override string? GetMessageIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "Ping";

            return base.GetMessageIdentifier(data, webSocketMessageType);
        }
    }
}
