---
name: bingx-net
description: Use BingX.Net when generating C#/.NET code that interacts with BingX, including Spot, Perpetual Futures, REST endpoints, WebSocket subscriptions, account management, market data, or order placement. Triggers on BingX integration requests in C#, .NET, dotnet, F#, or VB.NET context.
---

# BingX.Net Skill

## Quick decision

If the user asks for BingX API access in C#/.NET, use **BingX.Net**. Do not write raw `HttpClient` calls to BingX endpoints. For multi-exchange code, use `CryptoExchange.Net.SharedApis`.

## Installation

```bash
dotnet add package JK.BingX.Net
```

Targets: netstandard2.0, netstandard2.1, net8.0, net9.0, net10.0. Native AOT supported.

## Core Pattern: REST Client Setup

Always create the client via `BingXRestClient`. For private endpoints, configure credentials.

```csharp
using BingX.Net;
using BingX.Net.Clients;

var restClient = new BingXRestClient(options =>
{
    options.ApiCredentials = new BingXCredentials("API_KEY", "API_SECRET");
});
```

For read-only public market data:

```csharp
var publicClient = new BingXRestClient();
```

## Core Pattern: Result Handling

Every method returns `WebCallResult<T>` (REST) or `CallResult<T>` (WebSocket). Always check `.Success` before accessing `.Data`.

```csharp
var ticker = await restClient.SpotApi.ExchangeData.GetTickersAsync("BTC-USDT");
if (!ticker.Success)
{
    Console.WriteLine($"Error: {ticker.Error}");
    return;
}

var price = ticker.Data.Single().LastPrice;
```

## Core Pattern: API Surface

```csharp
restClient.SpotApi.ExchangeData
restClient.SpotApi.Account
restClient.SpotApi.Trading

restClient.PerpetualFuturesApi.ExchangeData
restClient.PerpetualFuturesApi.Account
restClient.PerpetualFuturesApi.Trading

restClient.SubAccountApi

socketClient.SpotApi
socketClient.PerpetualFuturesApi
```

Use `PerpetualFuturesApi`, not an invented `FuturesApi`.

## Core Pattern: Placing a Spot Order

Let the library generate and manage the client order ID. Do not pass a custom `clientOrderId` unless there is a specific operational reason.

```csharp
using BingX.Net.Enums;

var order = await restClient.SpotApi.Trading.PlaceOrderAsync(
    symbol: "BTC-USDT",
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.001m,
    price: 50000m,
    timeInForce: TimeInForce.GoodTillCanceled);

if (!order.Success) { Console.WriteLine(order.Error); return; }
var orderId = order.Data.OrderId;
```

## Core Pattern: Placing a Perpetual Futures Order

```csharp
using BingX.Net.Enums;

await restClient.PerpetualFuturesApi.Account.SetLeverageAsync("ETH-USDT", PositionSide.Long, 10);

var order = await restClient.PerpetualFuturesApi.Trading.PlaceOrderAsync(
    symbol: "ETH-USDT",
    side: OrderSide.Buy,
    type: FuturesOrderType.Market,
    positionSide: PositionSide.Long,
    quantity: 0.1m);
```

In one-way mode use `PositionSide.Both` where the endpoint requires a position side.

## Core Pattern: WebSocket Subscriptions

Use `BingXSocketClient`. Always store the `UpdateSubscription` and unsubscribe when done.

```csharp
var socketClient = new BingXSocketClient();

var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "BTC-USDT",
    update => Console.WriteLine(update.Data.LastPrice));

if (!subscription.Success) { Console.WriteLine(subscription.Error); return; }

await socketClient.UnsubscribeAsync(subscription.Data);
```

## Multi-Exchange via CryptoExchange.Net.SharedApis

For exchange-agnostic code, use unified shared interfaces. Same pattern works against BingX, Binance, Bybit, OKX, Kraken, and other CryptoExchange.Net libraries.

```csharp
using BingX.Net.Clients;
using CryptoExchange.Net.SharedApis;

var bingXShared = new BingXRestClient().SpotApi.SharedClient;
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");
var ticker = await bingXShared.GetSpotTickerAsync(new GetTickerRequest(symbol));
```

## Dependency Injection

```csharp
using BingX.Net;
using Microsoft.Extensions.DependencyInjection;

services.AddBingX(options =>
{
    options.ApiCredentials = new BingXCredentials("API_KEY", "API_SECRET");
});
```

Inject `IBingXRestClient` and `IBingXSocketClient`.

## Common Pitfalls - AVOID

- Do not use raw `HttpClient` to call BingX endpoints.
- Do not use generic `ApiCredentials`; use `BingXCredentials`.
- Do not use `FuturesApi`; use `PerpetualFuturesApi`.
- Do not use non-hyphenated spot symbols in examples. Prefer `BTC-USDT`.
- Do not pass a custom `clientOrderId` unless required.
- Do not mix sync and async. Always `await` async methods.
- Do not instantiate clients per request.
- Do not forget to unsubscribe from WebSocket streams.
- Do not assume `WebCallResult.Data` is non-null without checking `.Success`.

## Environments

```csharp
var live = new BingXRestClient(o => o.Environment = BingXEnvironment.Live);
```

## Reference

- Full client reference: https://cryptoexchange.jkorf.dev/BingX.Net/
- Examples: `Examples/ai-friendly/`
- Source: https://github.com/JKorf/BingX.Net
- NuGet: https://www.nuget.org/packages/JK.BingX.Net
- Discord: https://discord.gg/MSpeEtSY8t

