// 03-websocket.cs
//
// Demonstrates: WebSocket subscriptions - public ticker and klines.
// Includes proper teardown.
//
// Setup: dotnet add package JK.BingX.Net

using BingX.Net.Clients;
using BingX.Net.Enums;

// Reuse a single client instance across subscriptions.
var socketClient = new BingXSocketClient();

var tickerSub = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "BTC-USDT",
    update =>
    {
        Console.WriteLine($"BTC: {update.Data.LastPrice} (24h vol {update.Data.Volume:F2})");
    });

if (!tickerSub.Success)
{
    Console.WriteLine($"Failed to subscribe ticker: {tickerSub.Error}");
    return;
}

var klineSub = await socketClient.SpotApi.SubscribeToKlineUpdatesAsync(
    "ETH-USDT",
    KlineInterval.OneMinute,
    update =>
    {
        var k = update.Data.Kline;
        Console.WriteLine($"ETH 1m update: O={k.OpenPrice} H={k.HighPrice} L={k.LowPrice} C={k.ClosePrice}");
    });

if (!klineSub.Success)
{
    Console.WriteLine($"Failed to subscribe klines: {klineSub.Error}");
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    return;
}

Console.WriteLine("Subscriptions active. Press Enter to teardown...");
Console.ReadLine();

await socketClient.UnsubscribeAsync(tickerSub.Data);
await socketClient.UnsubscribeAsync(klineSub.Data);

Console.WriteLine("Clean shutdown complete.");

// Common variations:
//   Spot price:       socketClient.SpotApi.SubscribeToPriceUpdatesAsync(symbol, handler)
//   Spot order book:  socketClient.SpotApi.SubscribeToPartialOrderBookUpdatesAsync(symbol, depth, handler)
//   Futures ticker:   socketClient.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync(symbol, handler)
//   Mark price:       socketClient.PerpetualFuturesApi.SubscribeToMarkPriceUpdatesAsync(symbol, handler)

