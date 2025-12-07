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

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("dataType"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("dataType")!
            },

            // Account updates have e on first level
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("e"),
                    new PropertyFieldReference("ac"),
                ],
                StaticIdentifier = "SNAPSHOTAC"
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("e"),
                    new PropertyFieldReference("a"),
                ],
                StaticIdentifier = "SNAPSHOTA"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("id")!
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
