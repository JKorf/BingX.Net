using BingX.Net.Objects.Internal;
using BingX.Net.Objects.Models;
using BingX.Net.Objects.Sockets;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.WebSockets;
using System.Text;
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
                Priority = 3,
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


        //public override MessageInfo GetMessageInfo(ReadOnlySpan<byte> data, WebSocketMessageType? webSocketMessageType)
        //{
        //    if (data.Length == 4)
        //        return new MessageInfo { Identifier = "Ping" };

        //    InitializeSearch();

        //    SearchResult searchResult = new SearchResult();
        //    var reader = new Utf8JsonReader(data);
        //    while (reader.Read())
        //    {
        //        if ((reader.TokenType == JsonTokenType.StartArray
        //            || reader.TokenType == JsonTokenType.StartObject)
        //            && reader.CurrentDepth == _maxSearchDepth)
        //        {
        //            reader.Skip();
        //            continue;
        //        }

        //        if (reader.TokenType != JsonTokenType.PropertyName)
        //            continue;

        //        bool written = false;
        //        foreach (var field in _searchFields)
        //        {
        //            if (reader.CurrentDepth == field.Field.Level 
        //                && reader.ValueTextEquals(field.Field.Name))
        //            {
        //                reader.Read();

        //                if (field.Field.Type == typeof(int))
        //                    searchResult.WriteInt(field.Field.Name, reader.GetInt32());
        //                else
        //                    searchResult.WriteString(field.Field.Name, reader.GetString()!);

        //                if (field.ForceEvaluator != null)
        //                {
        //                    // Force the immediate return upon encountering this field
        //                    return new MessageInfo
        //                    {
        //                        DeserializationType = field.ForceEvaluator.TypeIdentifier(searchResult),
        //                        Identifier = field.ForceEvaluator.MessageIdentifier(searchResult)
        //                    };
        //                }

        //                written = true;
        //                break;
        //            }
        //        }

        //        if (written && _topEvaluator.Statisfied(searchResult))
        //            return _topEvaluator.ProduceMessageInfo(searchResult);

        //        if (written && _searchFields.All(x => searchResult.Contains(x.Field.Name)))
        //            break;
        //    }

        //    foreach(var evaluator in _messageEvaluators)
        //    {
        //        if (evaluator.Statisfied(searchResult))
        //            return evaluator.ProduceMessageInfo(searchResult);
        //    }

        //    return new MessageInfo();

        //    //string? eventType = null;
        //    //string? dataType = null;
        //    //DeserializationConfig? config = null;

        //    //bool CanReturn(DeserializationConfig? config)
        //    //{
        //    //    if (config == null || eventType == null)
        //    //        return false;

        //    //    return config.DataTypeRequired == (dataType != null);
        //    //}

        //    //while (reader.Read())
        //    //{
        //    //    if (reader.TokenType == JsonTokenType.PropertyName)
        //    //    {
        //    //        if (reader.CurrentDepth == 1 && reader.ValueTextEquals("id"))
        //    //        {
        //    //            // Query response
        //    //            reader.Read();
        //    //            return new MessageInfo { DeserializationType = typeof(BingXSocketResponse), Identifier = reader.GetString()! };
        //    //        }

        //    //        if (reader.CurrentDepth == 1 && reader.ValueTextEquals("ping"))
        //    //        {
        //    //            // Query response
        //    //            reader.Read();
        //    //            return new MessageInfo { DeserializationType = typeof(BingXPing), Identifier = "ping" };
        //    //        }

        //    //        if (reader.CurrentDepth == 1 && reader.ValueTextEquals("dataType"))
        //    //        {
        //    //            reader.Read();
        //    //            dataType = reader.GetString();

        //    //            if (CanReturn(config))
        //    //                return new MessageInfo { DeserializationType = config!.DeserializationType, Identifier = dataType != null ? dataType! : eventType };
        //    //        }
        //    //        else if (
        //    //            (reader.CurrentDepth == 1 || reader.CurrentDepth == 2)
        //    //            && reader.ValueTextEquals("e"))
        //    //        {
        //    //            // Event
        //    //            reader.Read();

        //    //            eventType = reader.GetString();
        //    //            DeserializationConfig? deserializationConfig = null;
        //    //            foreach (var item in _deserializationTypeMap)
        //    //            {
        //    //                if (reader.ValueSpan.SequenceEqual(item.Key))
        //    //                {
        //    //                    deserializationConfig = item.Value;
        //    //                    break;
        //    //                }
        //    //            }

        //    //            if (deserializationConfig == null)
        //    //            {
        //    //                // Error
        //    //                return new MessageInfo();
        //    //            }

        //    //            config = deserializationConfig;
        //    //            if (CanReturn(deserializationConfig))
        //    //                return new MessageInfo { DeserializationType = deserializationConfig.DeserializationType, Identifier = dataType != null ? dataType!: eventType };
        //    //        }
        //    //    }
        //    //}

        //    //return new MessageInfo { Identifier = dataType };
        //}
    }
}
