using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json;

namespace BingX.Net.Clients.MessageHandlers
{
    internal class BingXSocketClientSpotApiMessageConverter : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BingXExchange._serializerContext);

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("dataType"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("dataType")!
            },

            // Field 'e' on the first level only means account update
            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("e"),
                ],
                StaticIdentifier = "ACCOUNT_UPDATE"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("ping"),
                ],
                StaticIdentifier = "ping"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("id")!,
            }
        ];
    }
}
