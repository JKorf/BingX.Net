using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using BingX.Net.Interfaces.Clients;
using BingX.Net.Objects.Models;
using BingX.Net.UnitTests.TestImplementations;

namespace BingX.Net.UnitTests
{
    internal class JsonSocketTests
    {
        [Test]
        public async Task ValidatBingXUpdateStreamJson()
        {
            //await TestFileToObject<BingXModel>(@"JsonResponses/Spot/Socket/BingXAsync.txt");
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

            var result = JsonConvert.DeserializeObject<T>(json);
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
