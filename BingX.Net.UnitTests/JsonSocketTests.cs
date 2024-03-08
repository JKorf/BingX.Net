using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Models;
using BingX.Net.UnitTests.TestImplementations;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace BingX.Net.UnitTests
{
    internal class JsonSocketTests
    {
        [Test]
        public async Task ValidateTradeUpdatesStreamJson()
        {
            await TestFileToObject<BingXTradeUpdate>(@"JsonResponses/Spot/Socket/SubscribeToTradeUpdatesAsync.txt");
        }

        [Test]
        public async Task ValidateTickerUpdatesStreamJson()
        {
            await TestFileToObject<BingXTickerUpdate>(@"JsonResponses/Spot/Socket/SubscribeToTickerUpdatesAsync.txt");
        }

        [Test]
        public async Task ValidateBookPriceUpdatesStreamJson()
        {
            await TestFileToObject<BingXBookTickerUpdate>(@"JsonResponses/Spot/Socket/SubscribeToBookPriceUpdatesAsync.txt");
        }

        [Test]
        public async Task ValidateKlineUpdatesStreamJson()
        {
            await TestFileToObject<BingXKlineUpdate>(@"JsonResponses/Spot/Socket/SubscribeToKlineUpdatesAsync.txt");
        }

        [Test]
        public async Task ValidateOrderBookUpdatesStreamJson()
        {
            await TestFileToObject<BingXOrderBook>(@"JsonResponses/Spot/Socket/SubscribeToPartialOrderBookUpdatesAsync.txt");
        }

        [Test]
        public async Task ValidatePriceUpdatesStreamJson()
        {
            await TestFileToObject<BingXPriceUpdate>(@"JsonResponses/Spot/Socket/SubscribeToPriceUpdatesAsync.txt");
        }

        private static async Task TestFileToObject<T>(string filePath, List<string> ignoreProperties = null)
        {
            var listener = new EnumValueTraceListener();
            Trace.Listeners.Add(listener);
            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string json;
            try
            {
                var file = File.OpenRead(Path.Combine(path, filePath));
                using var reader = new StreamReader(file);
                json = await reader.ReadToEndAsync();
            }
            catch (FileNotFoundException)
            {
                throw;
            }

            var result = JsonSerializer.Deserialize<T>(json, SerializerOptions.WithConverters);
            JsonToObjectComparer<IBingXSocketClient>.ProcessData("", result, json, ignoreProperties: new Dictionary<string, List<string>>
            {
                { "", ignoreProperties ?? new List<string>() }
            });
            Trace.Listeners.Remove(listener);
        }
    }

    internal class EnumValueTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }

        public override void WriteLine(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }
    }
}
