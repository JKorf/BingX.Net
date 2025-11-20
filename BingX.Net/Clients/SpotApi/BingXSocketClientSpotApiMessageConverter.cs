using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json;

namespace BingX.Net.Clients.SpotApi
{
    internal class BingXSocketClientSpotApiMessageConverter : DynamicJsonConverter
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

            // Field 'e' on the first level only means account update
            new MessageEvaluator {
                Priority = 2,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("e"),
                ],
                StaticIdentifier = "ACCOUNT_UPDATE"
            },

            new MessageEvaluator {
                Priority = 3,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("ping"),
                ],
                StaticIdentifier = "ping"
            },

            new MessageEvaluator {
                Priority = 4,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("id"),
            }
        ];
    }
}
