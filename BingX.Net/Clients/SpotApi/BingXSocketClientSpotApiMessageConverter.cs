using BingX.Net.Objects.Internal;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Sockets;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
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
                MessageIdentifier = x => x.FieldValue("dataType")
            },

            // Field 'e' on the first level only means account update
            new MessageEvaluator {
                Priority = 3,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("e"),
                ],
                MessageIdentifier = x => "ACCOUNT_UPDATE"
            },

            new MessageEvaluator {
                Priority = 4,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("ping"),
                ],
                MessageIdentifier = x => "ping"
            },

            new MessageEvaluator {
                Priority = 5,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("id"),
                ],
                MessageIdentifier = x => x.FieldValue("id"),
            }
        ];



        //public override MessageInfo GetMessageInfo(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        //{
        //    var reader = new Utf8JsonReader(data);
        //    string? eventType = null;
        //    string? dataType = null;
        //    DeserializationConfig? config = null;

        //    bool CanReturn(DeserializationConfig? config)
        //    {
        //        if (config == null || eventType == null)
        //            return false;

        //        return config.DataTypeRequired == (dataType != null);
        //    }

        //    while (reader.Read())
        //    {
        //        if (reader.TokenType == JsonTokenType.PropertyName)
        //        {
        //            if (reader.CurrentDepth == 1 && reader.ValueTextEquals("id"))
        //            {
        //                // Query response
        //                reader.Read();
        //                return new MessageInfo { DeserializationType = typeof(BingXSocketResponse), Identifier = reader.GetString()! };
        //            }

        //            if (reader.CurrentDepth == 1 && reader.ValueTextEquals("ping"))
        //            {
        //                // Query response
        //                reader.Read();
        //                return new MessageInfo { DeserializationType = typeof(BingXPing), Identifier = "ping" };
        //            }

        //            if (reader.CurrentDepth == 1 && reader.ValueTextEquals("dataType"))
        //            {
        //                reader.Read();
        //                dataType = reader.GetString();

        //                if (CanReturn(config))
        //                    return new MessageInfo { DeserializationType = config!.DeserializationType, Identifier = dataType != null ? dataType! : eventType };
        //            }
        //            else if (
        //                (reader.CurrentDepth == 1 || reader.CurrentDepth == 2)
        //                && reader.ValueTextEquals("e"))
        //            {
        //                // Event
        //                reader.Read();

        //                eventType = reader.GetString();
        //                DeserializationConfig? deserializationConfig = null;
        //                foreach (var item in _deserializationTypeMap)
        //                {
        //                    if (reader.ValueSpan.SequenceEqual(item.Key))
        //                    {
        //                        deserializationConfig = item.Value;
        //                        break;
        //                    }
        //                }

        //                if (deserializationConfig == null)
        //                {
        //                    // Error
        //                    return new MessageInfo();
        //                }

        //                config = deserializationConfig;
        //                if (CanReturn(deserializationConfig))
        //                    return new MessageInfo { DeserializationType = deserializationConfig.DeserializationType, Identifier = dataType != null ? dataType!: eventType };
        //            }
        //        }
        //    }

        //    return new MessageInfo { Identifier = dataType };
        //}
    }
}
