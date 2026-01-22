using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BingX.Net;
using BingX.Net.Clients;
using BingX.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BingX.Net.Benchmark.Client
{
    [MemoryDiagnoser]
    //[ThreadingDiagnoser]
    //[SimpleJob(RuntimeMoniker.Net48)]
    //[SimpleJob(RuntimeMoniker.Net90)]
    [SimpleJob(RuntimeMoniker.Net10_0)]
    public class SocketTests
    {
        public BingXSocketClient Client;
        public ILogger Logger;

        private const int _receiveTarget = 1000; // Should match the number in the server


        [GlobalSetup]
        public void GlobalSetupNew()
        {
            CreateClient();
        }

        [Benchmark()]
        public async Task Normal()
        {
            var waitEvent = new AsyncResetEvent(false, false);
            var received = 0;
            var result = await Client.SpotApi.SubscribeToTradeUpdatesAsync("ETH-USDT", x =>
            {
                received++;
                if (received == _receiveTarget)
                    waitEvent.Set();

            }, CancellationToken.None);

            await waitEvent.WaitAsync();
            await result.Data.CloseAsync();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Client.Dispose();
        }

        private void CreateClient()
        {
            var logger = new LoggerFactory();
            //logger.AddProvider(new TraceLoggerProvider());
            Logger = logger.CreateLogger("Test");
            Client = new BingXSocketClient(Options.Create(new BingXSocketOptions
            {
                ReconnectPolicy = ReconnectPolicy.Disabled,
                RateLimiterEnabled = false,
                Environment = BingXEnvironment.CreateCustom("Benchmark", "", "ws://localhost:5034", "")
            }), logger);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // For manual testing:

            //var test = new SocketTests();
            //test.GlobalSetupNew();
            //Console.ReadLine();
            //Console.WriteLine("Starting");
            //for (var i = 0; i < 2; i++)
            //{
            //    test.NormalNew().Wait();
            //    Console.WriteLine(i);
            //}
            //Console.WriteLine("Finished");
            //test.GlobalCleanup();
            //Console.ReadLine();

            BenchmarkRunner.Run<SocketTests>();
        }
    }
}