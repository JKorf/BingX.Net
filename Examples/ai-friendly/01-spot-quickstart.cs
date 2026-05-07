// 01-spot-quickstart.cs
//
// Demonstrates: client setup, public market data, authenticated balances,
// limit order placement, order status check.
//
// Setup:
//   dotnet new console -n SpotQuickstart && cd SpotQuickstart
//   dotnet add package JK.BingX.Net
//   Copy this file content into Program.cs
//   Substitute API_KEY / API_SECRET below
//   dotnet run

using BingX.Net;
using BingX.Net.Clients;
using BingX.Net.Enums;

// ---- 1. PUBLIC CLIENT (no credentials needed for market data) ----
// Reuse this client across the application; do not create per request.
var publicClient = new BingXRestClient();

var ticker = await publicClient.SpotApi.ExchangeData.GetTickersAsync("BTC-USDT");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

var btcTicker = ticker.Data.Single();
Console.WriteLine($"BTC/USDT last price: {btcTicker.LastPrice}");
Console.WriteLine($"24h volume: {btcTicker.Volume} BTC");

// ---- 2. AUTHENTICATED CLIENT (for account / trading) ----
var tradingClient = new BingXRestClient(options =>
{
    options.ApiCredentials = new BingXCredentials("API_KEY", "API_SECRET");
});

var balances = await tradingClient.SpotApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine($"Failed to get balances: {balances.Error}");
    return;
}

foreach (var balance in balances.Data.Where(b => b.Total > 0))
{
    Console.WriteLine($"{balance.Asset}: {balance.Free} free, {balance.Locked} locked");
}

// ---- 3. PLACE A LIMIT BUY ORDER ----
// Limit, Buy, 0.001 BTC at a price 5% below current, likely not filled immediately.
// Let BingX.Net auto-generate clientOrderId unless you have a specific need.
var safePrice = Math.Round(btcTicker.LastPrice * 0.95m, 2);

var order = await tradingClient.SpotApi.Trading.PlaceOrderAsync(
    symbol: "BTC-USDT",
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.001m,
    price: safePrice,
    timeInForce: TimeInForce.GoodTillCanceled);

if (!order.Success)
{
    Console.WriteLine($"Failed to place order: {order.Error}");
    return;
}

Console.WriteLine($"Placed order {order.Data.OrderId} at {safePrice}, status: {order.Data.Status}");

// ---- 4. CHECK ORDER STATUS ----
var status = await tradingClient.SpotApi.Trading.GetOrderAsync("BTC-USDT", order.Data.OrderId);
if (status.Success)
{
    Console.WriteLine($"Order status: {status.Data.Status}, filled: {status.Data.QuantityFilled}");
}

// ---- 5. CANCEL THE ORDER (cleanup for this example) ----
var cancel = await tradingClient.SpotApi.Trading.CancelOrderAsync("BTC-USDT", order.Data.OrderId);
if (cancel.Success)
{
    Console.WriteLine($"Cancelled order {order.Data.OrderId}");
}

// Common variations:
//   Market order: type: OrderType.Market, omit price and timeInForce
//   Stop order:   add stopPrice parameter
//   Quote amount: use quoteQuantity parameter instead of quantity for market buys
//   OCO:          tradingClient.SpotApi.Trading.PlaceOcoOrderAsync(...)

