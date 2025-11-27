using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Net.WebSockets;
using System.Text.Json;

namespace BingX.Net.Clients.SpotApi
{
    internal class BingXSocketPerpetualFuturesMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BingXExchange._serializerContext);

        protected override MessageEvaluator[] TypeEvaluators { get; } = [

            new MessageEvaluator {
                Priority = 1,
                Fields = [
                    new PropertyFieldReference("dataType"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("dataType")!
            },

            // Account updates have e on first level
            new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("e"),
                    new PropertyFieldReference("ac"),
                ],
                StaticIdentifier = "SNAPSHOTAC"
            },
            new MessageEvaluator {
                Priority = 3,
                Fields = [
                    new PropertyFieldReference("e"),
                    new PropertyFieldReference("a"),
                ],
                StaticIdentifier = "SNAPSHOTA"
            },

            new MessageEvaluator {
                Priority = 4,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("id")!
            }
        ];

        public override string? GetTypeIdentifier(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        {
            if (data.Length == 4)
                return "Ping";

            return base.GetTypeIdentifier(data, webSocketMessageType);
        }
    }
}
