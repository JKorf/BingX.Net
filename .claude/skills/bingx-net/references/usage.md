# BingX.Net usage

These snippets use the imports below. Account and trading snippets assume an authenticated `client`. See [API surfaces](api-surfaces.md) for method routing and [operational considerations](safety.md) for trading and retry decisions.

```csharp
using System;
using System.Threading.Tasks;
using BingX.Net;
using BingX.Net.Clients;
using BingX.Net.Enums;
using BingX.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
```

## Authentication

Public calls can use `new BingXRestClient()`. Private calls require HMAC API-key credentials:

```csharp
using var client = new BingXRestClient(options =>
{
    options.ApiCredentials = new BingXCredentials("API_KEY", "API_SECRET");
});
```

The strings are placeholders. For key permissions and storage, see [credentials](safety.md#credentials).

## REST results

```csharp
using var client = new BingXRestClient();
var ticker = await client.SpotApi.ExchangeData.GetTickersAsync("BTC-USDT");
if (!ticker.Success)
{
    Console.WriteLine(ticker.Error);
    return;
}

Console.WriteLine(ticker.Data.Single().LastPrice);
```

On failure, diagnostic fields include `ErrorCode`, `Message`, `ErrorType`, and `IsTransient`. See [retry decisions](safety.md#retry-decisions) before adding retries.

## Account and spot orders

```csharp
var balances = await client.SpotApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}

foreach (var balance in balances.Data.Where(x => x.Total > 0))
    Console.WriteLine($"{balance.Asset}: {balance.Free} free, {balance.Locked} locked");
```

```csharp
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "BTC-USDT",
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.001m,
    price: 50000m,
    timeInForce: TimeInForce.GoodTillCanceled);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}

Console.WriteLine(order.Data.OrderId);
```

Query or cancel with `GetOrderAsync(symbol, orderId: ...)` or `CancelOrderAsync(symbol, orderId: ...)`. Inspect each result. The [API map](api-surfaces.md#spot-rest) lists related operations.

## Perpetual futures

```csharp
const string symbol = "ETH-USDT";
var leverage = await client.PerpetualFuturesApi.Account.SetLeverageAsync(symbol, PositionSide.Long, 5);
if (!leverage.Success)
{
    Console.WriteLine(leverage.Error);
    return;
}

var order = await client.PerpetualFuturesApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Buy,
    type: FuturesOrderType.Market,
    positionSide: PositionSide.Long,
    quantity: 0.01m);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}

Console.WriteLine(order.Data.OrderId);
```

The example assumes hedge mode. `positionSide` is required: use `Long`/`Short` in hedge mode or `Both` in one-way mode. Margin configuration uses `SetMarginModeAsync(symbol, MarginMode.Isolated)`. Check the account's position mode before constructing a close; see [trading and account changes](safety.md#trading-and-account-changes).

## WebSocket lifecycle

```csharp
using var socket = new BingXSocketClient();
var subscription = await socket.SpotApi.SubscribeToTickerUpdatesAsync(
    "BTC-USDT",
    update => Console.WriteLine(update.Data.LastPrice));

if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}

try
{
    Console.ReadLine(); // Replace with the application's lifetime.
}
finally
{
    await socket.UnsubscribeAsync(subscription.Data);
}
```

For spot klines, `SubscribeToKlineUpdatesAsync(symbol, interval, handler)` delivers the candle at `update.Data.Kline`. Perpetual kline callbacks receive an array in `update.Data`. Mark-price subscriptions use `SubscribeToMarkPriceUpdatesAsync(symbol, handler)`. See [socket routing](api-surfaces.md#perpetual-futures-websocket) and [private streams](api-surfaces.md#user-stream-lifecycle).

## Shared API V2

```csharp
using var client = new BingXRestClient();
IGetTickerRest ticker = client.SpotApi.SharedApi;
var result = await ticker.GetTickerAsync(
    new GetTickerRequest(new SharedSymbol(TradingMode.Spot, "BTC", "USDT")));

if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

Console.WriteLine(result.Data.LastPrice);
```

For aggregate properties and runtime capability lookup, see [Shared APIs](api-surfaces.md#shared-api-v2).

## Dependency injection

```csharp
var services = new ServiceCollection();
services.AddBingX();
```

For private calls, configure `options.ApiCredentials` in the `AddBingX(options => ...)` callback with the credentials shown in [authentication](#authentication). Inject `IBingXRestClient`, `IBingXSocketClient`, or `IBingXSharedApiClient`.

## Complete examples in a source checkout

These links require the repository checkout; the snippets above are available in the packaged skill itself.

- [Spot quickstart](../../../../Examples/ai-friendly/01-spot-quickstart.cs)
- [Futures](../../../../Examples/ai-friendly/02-perpetual-futures.cs)
- [WebSockets](../../../../Examples/ai-friendly/03-websocket.cs)
- [Multi-exchange](../../../../Examples/ai-friendly/04-multi-exchange.cs)
- [Error handling](../../../../Examples/ai-friendly/05-error-handling.cs)
