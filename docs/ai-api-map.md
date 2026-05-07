# BingX.Net AI API Quick Map

Use this file to route common user intents to the correct BingX.Net client member. If a method name or parameter is not listed here, inspect `BingX.Net/Interfaces/Clients/**` before generating code.

## Client Roots

| Intent | Use |
|---|---|
| REST calls | `new BingXRestClient()` |
| WebSocket streams | `new BingXSocketClient()` |
| API key authentication | `options.ApiCredentials = new BingXCredentials("key", "secret")` |
| Live environment | `BingXEnvironment.Live` |
| Dependency injection | `services.AddBingX(options => { ... })` |
| Spot REST | `client.SpotApi` |
| Perpetual Futures REST | `client.PerpetualFuturesApi` |
| Sub-account REST | `client.SubAccountApi` |
| Spot socket | `socketClient.SpotApi` |
| Perpetual Futures socket | `socketClient.PerpetualFuturesApi` |

## Symbols

BingX.Net examples should use hyphenated symbols such as `BTC-USDT` and `ETH-USDT`.

## Spot REST

| User intent | BingX.Net member |
|---|---|
| Get server time | `client.SpotApi.ExchangeData.GetServerTimeAsync()` |
| Get spot symbols | `client.SpotApi.ExchangeData.GetSymbolsAsync()` |
| Get one spot symbol | `client.SpotApi.ExchangeData.GetSymbolsAsync("BTC-USDT")` |
| Get latest spot ticker | `client.SpotApi.ExchangeData.GetTickersAsync("BTC-USDT")` |
| Get all spot tickers | `client.SpotApi.ExchangeData.GetTickersAsync()` |
| Get spot last trade price | `client.SpotApi.ExchangeData.GetLastTradeAsync("BTC-USDT")` |
| Get all spot last trade prices | `client.SpotApi.ExchangeData.GetLastTradesAsync()` |
| Get spot order book | `client.SpotApi.ExchangeData.GetOrderBookAsync("BTC-USDT")` |
| Get aggregated spot order book | `client.SpotApi.ExchangeData.GetAggregatedOrderBookAsync("BTC-USDT", limit, mergeDepth)` |
| Get recent trades | `client.SpotApi.ExchangeData.GetRecentTradesAsync("BTC-USDT")` |
| Get historical trades | `client.SpotApi.ExchangeData.GetTradeHistoryAsync("BTC-USDT")` |
| Get klines/candles | `client.SpotApi.ExchangeData.GetKlinesAsync("BTC-USDT", KlineInterval.OneMinute)` |
| Get historical klines | `client.SpotApi.ExchangeData.GetHistoricalKlinesAsync("BTC-USDT", KlineInterval.OneMinute)` |
| Get book price | `client.SpotApi.ExchangeData.GetBookPriceAsync("BTC-USDT")` |
| Get spot balances | `client.SpotApi.Account.GetBalancesAsync()` |
| Get funding balances | `client.SpotApi.Account.GetFundingBalancesAsync()` |
| Get deposit history | `client.SpotApi.Account.GetDepositHistoryAsync(...)` |
| Get withdrawal history | `client.SpotApi.Account.GetWithdrawalHistoryAsync(...)` |
| Get assets | `client.SpotApi.Account.GetAssetsAsync()` |
| Withdraw asset | `client.SpotApi.Account.WithdrawAsync(...)` |
| Get deposit address | `client.SpotApi.Account.GetDepositAddressAsync(asset)` |
| Transfer assets | `client.SpotApi.Account.TransferAsync(...)` |
| Get transfer history | `client.SpotApi.Account.GetTransfersAsync(...)` |
| Internal transfer | `client.SpotApi.Account.TransferInternalAsync(...)` |
| Get internal transfer history | `client.SpotApi.Account.GetInternalTransfersAsync(...)` |
| Start spot user stream | `client.SpotApi.Account.StartUserStreamAsync()` |
| Keep alive spot user stream | `client.SpotApi.Account.KeepAliveUserStreamAsync(listenKey)` |
| Stop spot user stream | `client.SpotApi.Account.StopUserStreamAsync(listenKey)` |
| Get spot trading fees | `client.SpotApi.Account.GetTradingFeesAsync("BTC-USDT")` |
| Get user id | `client.SpotApi.Account.GetUserIdAsync()` |
| Get API key permissions | `client.SpotApi.Account.GetApiKeyPermissionsAsync(userId)` |
| Place spot order | `client.SpotApi.Trading.PlaceOrderAsync(...)` |
| Place multiple spot orders | `client.SpotApi.Trading.PlaceMultipleOrdersAsync(...)` |
| Query spot order | `client.SpotApi.Trading.GetOrderAsync(symbol, orderId)` |
| Get open spot orders | `client.SpotApi.Trading.GetOpenOrdersAsync(symbol)` |
| Get spot order history | `client.SpotApi.Trading.GetOrdersAsync(...)` |
| Cancel spot order | `client.SpotApi.Trading.CancelOrderAsync(symbol, orderId)` |
| Cancel multiple spot orders | `client.SpotApi.Trading.CancelOrdersAsync(...)` |
| Cancel all spot orders | `client.SpotApi.Trading.CancelAllOrdersAsync(symbol)` |
| Dead man's switch | `client.SpotApi.Trading.CancelAllOrdersAfterAsync(...)` |
| Get spot user trades | `client.SpotApi.Trading.GetUserTradesAsync(symbol)` |
| Place OCO order | `client.SpotApi.Trading.PlaceOcoOrderAsync(...)` |
| Cancel OCO order | `client.SpotApi.Trading.CancelOcoOrderAsync(...)` |
| Get OCO order | `client.SpotApi.Trading.GetOcoOrderAsync(...)` |
| Get open OCO orders | `client.SpotApi.Trading.GetOpenOcoOrdersAsync(page, pageSize)` |

## Perpetual Futures REST

| User intent | BingX.Net member |
|---|---|
| Get futures server time | `client.PerpetualFuturesApi.ExchangeData.GetServerTimeAsync()` |
| Get contracts | `client.PerpetualFuturesApi.ExchangeData.GetContractsAsync()` |
| Get one contract | `client.PerpetualFuturesApi.ExchangeData.GetContractsAsync("ETH-USDT")` |
| Get futures ticker | `client.PerpetualFuturesApi.ExchangeData.GetTickerAsync("ETH-USDT")` |
| Get all futures tickers | `client.PerpetualFuturesApi.ExchangeData.GetTickersAsync()` |
| Get futures last trade price | `client.PerpetualFuturesApi.ExchangeData.GetLastTradePriceAsync("ETH-USDT")` |
| Get all futures last trade prices | `client.PerpetualFuturesApi.ExchangeData.GetLastTradePricesAsync()` |
| Get futures order book | `client.PerpetualFuturesApi.ExchangeData.GetOrderBookAsync("ETH-USDT")` |
| Get recent futures trades | `client.PerpetualFuturesApi.ExchangeData.GetRecentTradesAsync("ETH-USDT")` |
| Get futures trade history | `client.PerpetualFuturesApi.ExchangeData.GetTradeHistoryAsync("ETH-USDT")` |
| Get futures klines | `client.PerpetualFuturesApi.ExchangeData.GetKlinesAsync("ETH-USDT", KlineInterval.OneMinute)` |
| Get mark price klines | `client.PerpetualFuturesApi.ExchangeData.GetMarkPriceKlinesAsync("ETH-USDT", KlineInterval.OneMinute)` |
| Get funding rate | `client.PerpetualFuturesApi.ExchangeData.GetFundingRateAsync("ETH-USDT")` |
| Get all funding rates | `client.PerpetualFuturesApi.ExchangeData.GetFundingRatesAsync()` |
| Get funding rate history | `client.PerpetualFuturesApi.ExchangeData.GetFundingRateHistoryAsync("ETH-USDT")` |
| Get open interest | `client.PerpetualFuturesApi.ExchangeData.GetOpenInterestAsync("ETH-USDT")` |
| Get book ticker | `client.PerpetualFuturesApi.ExchangeData.GetBookTickerAsync("ETH-USDT")` |
| Get trading rules | `client.PerpetualFuturesApi.ExchangeData.GetTradingRulesAsync("ETH-USDT")` |
| Get futures balances | `client.PerpetualFuturesApi.Account.GetBalancesAsync()` |
| Get income history | `client.PerpetualFuturesApi.Account.GetIncomesAsync(...)` |
| Get futures trading fees | `client.PerpetualFuturesApi.Account.GetTradingFeesAsync()` |
| Start futures user stream | `client.PerpetualFuturesApi.Account.StartUserStreamAsync()` |
| Keep alive futures user stream | `client.PerpetualFuturesApi.Account.KeepAliveUserStreamAsync(listenKey)` |
| Stop futures user stream | `client.PerpetualFuturesApi.Account.StopUserStreamAsync(listenKey)` |
| Get margin mode | `client.PerpetualFuturesApi.Account.GetMarginModeAsync(symbol)` |
| Set margin mode | `client.PerpetualFuturesApi.Account.SetMarginModeAsync(symbol, MarginMode.Isolated)` |
| Get leverage | `client.PerpetualFuturesApi.Account.GetLeverageAsync(symbol)` |
| Set leverage | `client.PerpetualFuturesApi.Account.SetLeverageAsync(symbol, PositionSide.Long, leverage)` |
| Adjust isolated margin | `client.PerpetualFuturesApi.Account.AdjustIsolatedMarginAsync(...)` |
| Get position mode | `client.PerpetualFuturesApi.Account.GetPositionModeAsync()` |
| Set position mode | `client.PerpetualFuturesApi.Account.SetPositionModeAsync(...)` |
| Get isolated margin history | `client.PerpetualFuturesApi.Account.GetIsolatedMarginChangeHistoryAsync(...)` |
| Apply for VST assets | `client.PerpetualFuturesApi.Account.ApplyForVSTAssetsAsync()` |
| Get multi-asset mode | `client.PerpetualFuturesApi.Account.GetMultiAssetModeAsync()` |
| Set multi-asset mode | `client.PerpetualFuturesApi.Account.SetMultiAssetModeAsync(...)` |
| Get multi-asset rules | `client.PerpetualFuturesApi.Account.GetMultiAssetRulesAsync()` |
| Get multi-assets margin | `client.PerpetualFuturesApi.Account.GetMultiAssetsMarginAsync()` |
| Get positions | `client.PerpetualFuturesApi.Trading.GetPositionsAsync(symbol)` |
| Place test futures order | `client.PerpetualFuturesApi.Trading.PlaceTestOrderAsync(...)` |
| Place futures order | `client.PerpetualFuturesApi.Trading.PlaceOrderAsync(...)` |
| Place multiple futures orders | `client.PerpetualFuturesApi.Trading.PlaceMultipleOrderAsync(...)` |
| Edit futures order | `client.PerpetualFuturesApi.Trading.EditOrderAsync(...)` |
| Query futures order | `client.PerpetualFuturesApi.Trading.GetOrderAsync(symbol, orderId)` |
| Cancel futures order | `client.PerpetualFuturesApi.Trading.CancelOrderAsync(symbol, orderId)` |
| Cancel multiple futures orders | `client.PerpetualFuturesApi.Trading.CancelMultipleOrderAsync(...)` |
| Cancel all futures orders | `client.PerpetualFuturesApi.Trading.CancelAllOrderAsync(...)` |
| Get open futures orders | `client.PerpetualFuturesApi.Trading.GetOpenOrdersAsync(symbol)` |
| Get closed futures orders | `client.PerpetualFuturesApi.Trading.GetClosedOrdersAsync(...)` |
| Get all futures orders | `client.PerpetualFuturesApi.Trading.GetOrdersAsync(...)` |
| Get liquidation orders | `client.PerpetualFuturesApi.Trading.GetLiquidationOrdersAsync(...)` |
| Get futures user trades | `client.PerpetualFuturesApi.Trading.GetUserTradesAsync(...)` |
| Close all positions | `client.PerpetualFuturesApi.Trading.CloseAllPositionsAsync(symbol)` |
| Close position by id | `client.PerpetualFuturesApi.Trading.ClosePositionAsync(positionId)` |
| Get position and margin info | `client.PerpetualFuturesApi.Trading.GetPositionAndMarginInfoAsync(symbol)` |
| Get position history | `client.PerpetualFuturesApi.Trading.GetPositionHistoryAsync(symbol)` |
| Place TWAP order | `client.PerpetualFuturesApi.Trading.PlaceTwapOrderAsync(...)` |
| Get open TWAP orders | `client.PerpetualFuturesApi.Trading.GetOpenTwapOrdersAsync(symbol)` |
| Get closed TWAP orders | `client.PerpetualFuturesApi.Trading.GetClosedTwapOrdersAsync(...)` |
| Cancel TWAP order | `client.PerpetualFuturesApi.Trading.CancelTwapOrderAsync(orderId)` |

## Spot WebSocket

| User intent | BingX.Net member |
|---|---|
| Subscribe spot ticker updates | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(symbol, handler)` |
| Subscribe spot price updates | `socketClient.SpotApi.SubscribeToPriceUpdatesAsync(symbol, handler)` |
| Subscribe spot book price updates | `socketClient.SpotApi.SubscribeToBookPriceUpdatesAsync(symbol, handler)` |
| Subscribe spot trades | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync(symbol, handler)` |
| Subscribe spot klines | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync(symbol, interval, handler)` |
| Subscribe spot partial order book | `socketClient.SpotApi.SubscribeToPartialOrderBookUpdatesAsync(symbol, depth, handler)` |
| Subscribe spot incremental order book | `socketClient.SpotApi.SubscribeToIncrementalOrderBookUpdatesAsync(symbol, handler)` |
| Subscribe spot order updates | `socketClient.SpotApi.SubscribeToOrderUpdatesAsync(listenKey, handler)` |
| Subscribe spot balance updates | `socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(listenKey, handler)` |

## Perpetual Futures WebSocket

| User intent | BingX.Net member |
|---|---|
| Subscribe futures ticker updates | `socketClient.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync(symbol, handler)` |
| Subscribe all futures ticker updates | `socketClient.PerpetualFuturesApi.SubscribeToTickerUpdatesAsync(handler)` |
| Subscribe futures price updates | `socketClient.PerpetualFuturesApi.SubscribeToPriceUpdatesAsync(symbol, handler)` |
| Subscribe futures mark price updates | `socketClient.PerpetualFuturesApi.SubscribeToMarkPriceUpdatesAsync(symbol, handler)` |
| Subscribe futures book price updates | `socketClient.PerpetualFuturesApi.SubscribeToBookPriceUpdatesAsync(symbol, handler)` |
| Subscribe futures trades | `socketClient.PerpetualFuturesApi.SubscribeToTradeUpdatesAsync(symbol, handler)` |
| Subscribe futures klines | `socketClient.PerpetualFuturesApi.SubscribeToKlineUpdatesAsync(symbol, interval, handler)` |
| Subscribe futures partial order book | `socketClient.PerpetualFuturesApi.SubscribeToPartialOrderBookUpdatesAsync(symbol, depth, updateInterval, handler)` |
| Subscribe futures incremental order book | `socketClient.PerpetualFuturesApi.SubscribeToIncrementalOrderBookUpdatesAsync(symbol, handler)` |
| Subscribe futures user data | `socketClient.PerpetualFuturesApi.SubscribeToUserDataUpdatesAsync(listenKey, ...)` |

## SharedApis

| User intent | BingX.Net member or interface |
|---|---|
| Shared spot REST client | `new BingXRestClient().SpotApi.SharedClient` |
| Shared perpetual futures REST client | `new BingXRestClient().PerpetualFuturesApi.SharedClient` |
| Shared spot socket client | `new BingXSocketClient().SpotApi.SharedClient` |
| Shared perpetual futures socket client | `new BingXSocketClient().PerpetualFuturesApi.SharedClient` |
| Shared spot ticker REST | `ISpotTickerRestClient.GetSpotTickerAsync(new GetTickerRequest(symbol))` |
| Shared spot order REST | `ISpotOrderRestClient.PlaceSpotOrderAsync(...)` |
| Shared futures order REST | `IFuturesOrderRestClient.PlaceFuturesOrderAsync(...)` |
| Shared ticker socket | `ITickerSocketClient.SubscribeToTickerUpdatesAsync(...)` |
| Shared order book socket | `IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(...)` |

For shared socket subscriptions, keep the concrete socket client and unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

## Result Handling

| Situation | Pattern |
|---|---|
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Retry decision | Retry only when `result.Error?.IsTransient == true` |
| Cancellation | Pass `ct: cancellationToken` |

## Common Routing Pitfalls

| Do not use | Use instead |
|---|---|
| Raw `HttpClient` | `BingXRestClient` / `BingXSocketClient` |
| `ApiCredentials` | `BingXCredentials` |
| `FuturesApi` | `PerpetualFuturesApi` |
| `SwapApi` | `PerpetualFuturesApi` |
| `BTCUSDT` in examples | `BTC-USDT` |
| `.Data` without `.Success` check | Check `.Success` first |
| Shared socket `UnsubscribeAsync(...)` | Keep the concrete socket client and call `socketClient.UnsubscribeAsync(subscription.Data)` |
| Custom `clientOrderId` by default | Let BingX.Net auto-generate it |
| `positionSide` omitted from futures orders | Futures `PlaceOrderAsync` requires `PositionSide` |

