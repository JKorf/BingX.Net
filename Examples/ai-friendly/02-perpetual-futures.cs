// 02-perpetual-futures.cs
//
// Demonstrates: Perpetual Futures - set leverage, place market order,
// retrieve open position, close position.
//
// Setup: dotnet add package JK.BingX.Net
// Substitute API_KEY / API_SECRET.

using BingX.Net;
using BingX.Net.Clients;
using BingX.Net.Enums;

var client = new BingXRestClient(options =>
{
    options.ApiCredentials = new BingXCredentials("API_KEY", "API_SECRET");
});

const string symbol = "ETH-USDT";

// ---- 1. SET LEVERAGE ----
// Leverage is per-symbol and side-specific in hedge mode.
var leverage = await client.PerpetualFuturesApi.Account.SetLeverageAsync(symbol, PositionSide.Long, 5);
if (!leverage.Success)
{
    Console.WriteLine($"Failed to set leverage: {leverage.Error}");
    return;
}
Console.WriteLine($"Leverage set for {symbol}");

// ---- 2. PLACE MARKET ORDER (open long position) ----
// In one-way mode use PositionSide.Both.
var openOrder = await client.PerpetualFuturesApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Buy,
    type: FuturesOrderType.Market,
    positionSide: PositionSide.Long,
    quantity: 0.01m);

if (!openOrder.Success)
{
    Console.WriteLine($"Failed to open position: {openOrder.Error}");
    return;
}
Console.WriteLine($"Opened position via order {openOrder.Data.OrderId}");

// ---- 3. GET CURRENT POSITION ----
var positions = await client.PerpetualFuturesApi.Trading.GetPositionsAsync(symbol);
if (!positions.Success)
{
    Console.WriteLine($"Failed to get positions: {positions.Error}");
    return;
}

var position = positions.Data.FirstOrDefault(p => p.Size != 0);
if (position == null)
{
    Console.WriteLine("No open position found (may not have filled yet).");
    return;
}

Console.WriteLine($"Position: {position.Size} {symbol} at avg {position.AveragePrice}");
Console.WriteLine($"Unrealized PnL: {position.UnrealizedProfit} {position.Currency}");
Console.WriteLine($"Liquidation price: {position.LiquidationPrice}");

// ---- 4. CLOSE THE POSITION ----
// Opposite side, same quantity, reduceOnly=true to avoid an accidental position flip.
var closeOrder = await client.PerpetualFuturesApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Sell,
    type: FuturesOrderType.Market,
    positionSide: PositionSide.Long,
    quantity: Math.Abs(position.Size),
    reduceOnly: true);

if (closeOrder.Success)
{
    Console.WriteLine($"Closed position via order {closeOrder.Data.OrderId}");
}

// Common variations:
//   Limit order: type: FuturesOrderType.Limit, add price + timeInForce
//   Stop-market: type: FuturesOrderType.StopMarket, add stopPrice
//   One-way mode: positionSide: PositionSide.Both
//   Margin mode: client.PerpetualFuturesApi.Account.SetMarginModeAsync(symbol, MarginMode.Isolated)

