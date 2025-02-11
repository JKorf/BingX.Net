# ![.BingX.Net](https://github.com/JKorf/BingX.Net/blob/781598404ac510d462c91e6888be2d530bd6e350/BingX.Net/Icon/BingX.png) BingX.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/BingX.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/BingX.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/BingX.Net?style=for-the-badge)

BingX.Net is a client library for accessing the [BingX REST and Websocket API](https://bingx-api.github.io/docs/#/en-us/swapV2/changelog).
## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Extensive logging
* Support for different environments
* Easy integration with other exchange client based on the CryptoExchange.Net base library

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=for-the-badge)](https://www.nuget.org/packages/JK.BingX.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/JK.BingX.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/JK.BingX.Net)

	dotnet add package JK.BingX.Net
	
### GitHub packages
BingX.Net is available on [GitHub packages](https://github.com/JKorf/BingX.Net/pkgs/nuget/JK.BingX.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/BingX.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/BingX.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/BingX.Net/releases).

		
## How to use
*REST Endpoints*  

```csharp
// Get the ETH/USDT ticker via rest request
var restClient = new BingXRestClient();
var tickerResult = await restClient.SpotApi.ExchangeData.GetTickersAsync("ETH-USDT");
var lastPrice = tickerResult.Data.Single().LastPrice;
```
	
*Websocket streams*  

```csharp
// Subscribe to ETH/USDT ticker updates via the websocket API
var socketClient = new BingXSocketClient();
var tickerSubscriptionResult = socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH-USDT", (update) =>
{
    var lastPrice = update.Data.LastPrice;
});
```

For information on the clients, dependency injection, response processing and more see the [BingX.Net documentation](https://jkorf.github.io/BingX.Net), [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net), or have a look at the examples [here](https://github.com/JKorf/BingX.Net/tree/main/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## CryptoExchange.Net
BingX.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://jkorf.github.io/CryptoExchange.Net#idocs_shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BitMEX|[JKorf/BitMEX.Net](https://github.com/JKorf/BitMEX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.BitMEX.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.Net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.Net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### Spot
|API|Supported|Location|
|--|--:|--|
|Market Interface|✓|`restClient.SpotApi.ExchangeData`|
|Wallet deposits and withdrawals|✓|`restClient.SpotApi.Account`|
|Spot account|✓|`restClient.SpotApi.Account`|
|Trade interface|✓|`restClient.SpotApi.Trading`|
|Websocket Market Data|✓|`socketClient.SpotApi`|
|Websocket Account Data|✓|`socketClient.SpotApi`|

### Perpetual Futures
|API|Supported|Location|
|--|--:|--|
|Market Interface|✓|`restClient.PerpetualFuturesApi.ExchangeData`|
|Account Interface|✓|`restClient.PerpetualFuturesApi.Account`|
|Trade Interface|✓|`restClient.PerpetualFuturesApi.Account`/`restClient.PerpetualFuturesApi.Trading`|
|Websocket Market Data|✓|`socketClient.PerpetualFuturesApi`|
|Websocket Account Data|✓|`socketClient.PerpetualFuturesApi`|

### Standard Contract
|API|Supported|Location|
|--|--:|--|
|Standard Contract Interface|X||

### Account & Wallet
|API|Supported|Location|
|--|--:|--|
|Spot account|✓|`restClient.SpotApi.Account`|
|Wallet deposits and withdrawals|✓|`restClient.SpotApi.Account`|
|Sub-account|X||

### Copy Trading
|API|Supported|Location|
|--|--:|--|
|Standard Contract Interface|X||

## Support the project
Any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1q277a5n54s2l2mzlu778ef7lpkwhjhyvghuv8qf  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 1.21.0 - 11 Feb 2025
    * Updated CryptoExchange.Net to version 8.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added support for more SharedKlineInterval values

* Version 1.20.1 - 06 Feb 2025
    * Added processPartialSuccess parameter to restClient.SpotApi.Trading.CancelOrdersAsync endpoint

* Version 1.20.0 - 05 Feb 2025
    * Updated CryptoExchange.Net to version 8.7.3, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added missing TimeInForce parameter for restClient.SpotApi.Trading.PlaceMultipleOrderAsync
    * Added ContractAddress property to BingXAsset model
    * Updated restClient.SpotApi.Trading.CancelOrdersAsync response model so failed cancellations can also be returned
    * Fixed clientOrderId parameter for restClient.PerpetualFuturesApi.Trading.PlaceOrderAsync endpoint

* Version 1.19.2 - 04 Feb 2025
    * Added AveragePrice to SpotApi order details model and shared implementat…

* Version 1.19.1 - 07 Jan 2025
    * Updated CryptoExchange.Net version
    * Added Type property to BingXExchange class

* Version 1.19.0 - 23 Dec 2024
    * Updated CryptoExchange.Net to version 8.5.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added SetOptions methods on Rest and Socket clients
    * Added setting of DefaultProxyCredentials to CredentialCache.DefaultCredentials on the DI http client
    * Improved websocket disconnect detection
    * Fixed BingXFuturesPlaceOrderRequest serialization

* Version 1.18.2 - 20 Dec 2024
    * Fixed bingXRestClient.PerpetualFuturesApi.Account.GetIncomesAsync parameter serialization

* Version 1.18.1 - 03 Dec 2024
    * Updated CryptoExchange.Net to version 8.4.3, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Fixed orderbook creation via BingXOrderBookFactory

* Version 1.18.0 - 28 Nov 2024
    * Updated CryptoExchange.Net to version 8.4.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.4.0
    * Added GetFeesAsync Shared REST client implementations
    * Updated BingXOptions to LibraryOptions implementation
    * Updated test and analyzer package versions

* Version 1.17.0 - 25 Nov 2024
    * Added restClient.PerpetualFuturesApi.Account.ApplyForVSTAssetsAsync
    * Added restClient.PerpetualFuturesApi.Account.SetMultiAssetModeAsync
    * Added restClient.PerpetualFuturesApi.Account.GetMultiAssetModeAsync
    * Added restClient.PerpetualFuturesApi.Account.GetMultiAssetRulesAsync
    * Added restClient.PerpetualFuturesApi.Account.GetMultiAssetsMarginAsync
    * Added restClient.PerpetualFuturesApi.Trading.GetOrdersAsync
    * Added restClient.PerpetualFuturesApi.Trading.PlaceTwapOrderAsync
    * Added restClient.PerpetualFuturesApi.Trading.GetOpenTwapOrdersAsync
    * Added restClient.PerpetualFuturesApi.Trading.GetTwapOrderAsync
    * Added restClient.PerpetualFuturesApi.Trading.GetClosedTwapOrdersAsync
    * Added restClient.PerpetualFuturesApi.Trading.CancelTwapOrderAsync
    * Added Demo BingXEnvironment
    * Added websocket connection limit perpetual futures
    * Fixed inverted order side for Shared trades

* Version 1.16.0 - 19 Nov 2024
    * Updated CryptoExchange.Net to version 8.3.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.3.0
    * Added support for loading client settings from IConfiguration
    * Added DI registration method for configuring Rest and Socket options at the same time
    * Added DisplayName and ImageUrl properties to BingXExchange class
    * Updated client constructors to accept IOptions from DI
    * Removed redundant BingXSocketClient constructor

* Version 1.15.1 - 15 Nov 2024
    * Fixed available balance response on shared futures implementation
    * Removed symbol parameters from GetPositionModeAsync and SetPositionModeAsync endpoints

* Version 1.15.0 - 13 Nov 2024
    * Added initial SubAccount API implementation

* Version 1.14.0 - 06 Nov 2024
    * Updated CryptoExchange.Net to version 8.2.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.2.0

* Version 1.13.0 - 04 Nov 2024
    * Added restClient.PerpetualFuturesApi.Account.GetIsolatedMarginChangeHistoryAsync endpoint
    * Added settleAsset parameter to FuturesApi.Trading endpoints

* Version 1.12.0 - 28 Oct 2024
    * Updated CryptoExchange.Net to version 8.1.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.1.0
    * Moved FormatSymbol to BingXExchange class
    * Added support Side setting on SharedTrade model
    * Added BingXTrackerFactory for creating trackers
    * Added overload to Create method on BingXOrderBookFactory support SharedSymbol parameter
    * Added Shared websocket kline subscription implementation for futures and spot APIs

* Version 1.11.2 - 21 Oct 2024
    * Added Tier property to restClient.PerpetualFuturesApi.Trading.GetPositionAndMarginInfoAsync response model
    * Added ReduceOnly property to socketClient.PerpetualFuturesApi.SubscribeToUserDataUpdatesAsync order update model

* Version 1.11.1 - 14 Oct 2024
    * Updated CryptoExchange.Net to version 8.0.3, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.0.3
    * Fixed TypeLoadException during initialization

* Version 1.11.0 - 27 Sep 2024
    * Updated CryptoExchange.Net to version 8.0.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.0.0
    * Added Shared client interfaces implementation for Spot and Perpetual Futures Rest and Socket clients
    * Added check for api credentials in rest user stream operations
    * Added PerpetualFuturesApi.Trading.GetUserTradesAsync endpoint
    * Added timeInForce parameter to SpotApi.Trading.PlaceOrderAsync endpoint
    * Updated fromId parameter on SpotApi.Trading.GetuserTradesAsync from int? to long?
    * Updated KlineInterval Enum values to match number of seconds
    * Updated Sourcelink package version
    * Marked ISpotClient references as deprecated

* Version 1.10.0 - 11 Sep 2024
    * Fixed request signing for requests with special characters
    * Added SpotApi Oco endpoints

* Version 1.9.2 - 28 Aug 2024
    * Updated CryptoExchange.Net to version 7.11.2, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.11.2
    * Fixed Boolean parameter serialization on PerpetualFuturesApi.Trading order endpoints
    * Added RealizedPnl property to PerpetualFuturesApi websocket position update
    * Added TimeOffline, TimeMaintenance properties to SpotApi symbol model
    * Added BrokerProhibited property to PerpetualFuturesApi contract response model

* Version 1.9.1 - 23 Aug 2024
    * Fixed enum type on OrderType property on socket perpetual futures order update

* Version 1.9.0 - 19 Aug 2024
    * Added PerpetualFuturesApi.Trading.GetPositionHistoryAsync endpoint
    * Updated PerpetualFuturesApi.Account.GetBalancesAsync to V3, returning both USDT and USDC balances
    * Added sync parameter to SpotApi.Trading.PlaceMultipleOrderAsync endpoint

* Version 1.8.0 - 07 Aug 2024
    * Updated CryptoExchange.Net to version 7.11.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.11.0
    * Updated XML code comments
    * Fixed PerpetualFuturesApi.Account.SetMarginModeAsync endpoint

* Version 1.7.0 - 27 Jul 2024
    * Updated CryptoExchange.Net to version 7.10.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.10.0

* Version 1.6.0 - 16 Jul 2024
    * Updated CryptoExchange.Net to version 7.9.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.9.0
    * Updated internal classes to internal access modifier
    * Added PerpetualFuturesApi.ExchangeData.GetTickersAsync endpoint
    * Added PerpetualFuturesApi.ExchangeData.GetLastTradePricesAsync endpoint
    * Added PerpetualFuturesApi.ExchangeData.GetFundingRatesAsync endpoint
    * Added SpotApi.ExchangeData.GetLastTradesAsync endpoint
    * Added SpotApi.Account.GetUserIdAsync endpoint
    * Added SpotApi.Account.GetApiKeyPermissionsAsync endpoint
    * Added sync parameter to SpotApi.Trading.PlaceMultipleOrdersAsync
    * Updated API endpoint docs references
    * Fixed Spot and Futures KeepAliveUserStreamAsync endpoint
    * Fixed clientOrderId deserialization in websocket order updates

* Version 1.5.0 - 02 Jul 2024
    * Updated CryptoExchange.Net to V7.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.8.0
    * Added TakeProfit/StopLoss parameters to perpetual futures order endpoints

* Version 1.4.0 - 26 Jun 2024
    * Updated CryptoExchange.Net to 7.7.2, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.7.2
    * Added rate limiting ratelimiting implementation
    * Updated BingXPosition model

* Version 1.3.2 - 23 Jun 2024
    * Updated CryptoExchange.Net to version 7.7.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.7.0

* Version 1.3.1 - 13 Jun 2024
    * Fixed bingXClient.PerpetualFuturesApi.ExchangeData.GetContractsAsync response parsing by updating Status mapping

* Version 1.3.0 - 11 Jun 2024
    * Updated CryptoExchange.Net to v7.6.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 1.2.0 - 02 Jun 2024
    * Added PerpetualFuturesApi.SubscribeToPartialOrderBookUpdatesAsync, PerpetualFuturesApi.SubscribeToKlineUpdatesAsync and PerpetualFuturesApi.SubscribeToTickerUpdatesAsync subscriptions for all symbols
    * Added PerpetualFuturesApi.Trading.GetPositionAndMarginInfoAsync endpoint
    * Added optional symbol parameter PerpetualFuturesApi.ExchangeData.GetContractsAsync
    * Updated BingXWithdrawal response model
    * Updated BingXPosition response model

* Version 1.1.1 - 07 May 2024
    * Updated CryptoExchange.Net to v7.5.2, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 1.1.0 - 02 May 2024
    * Removed need for API credentials in certain ExchangeData calls
    * Renamed PerpetualFutures.Trading.GetClosedOrderAsync to GetClosedOrdersAsync
    * Changed PerpetualFutures.SubscribeToUserDataUpdatesAsync handlers to be nullable
    * Fixed SpotApi.SubscribeToBalanceUpdatesAsync update handling
    * Various small fixes

* Version 1.0.5 - 01 May 2024
    * Updated CryptoExchange.Net to v7.5.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 1.0.4 - 28 Apr 2024
    * Added BingXExchange static info class
    * Added BingXOrderBookFactory book creation method
    * Fixed BingXOrderBookFactory injection issue
    * Updated CryptoExchange.Net to v7.4.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 1.0.3 - 23 Apr 2024
    * Updated CryptoExchange.Net to 7.3.3, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 1.0.2 - 18 Apr 2024
    * Updated CryptoExchange.Net to 7.3.1, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 1.0.1 - 10 Apr 2024
    * Added PostOnly TimeInForce enum value
    * Updated PerpetualFuturesApi.ExchangeData.GetContractsAsync response model
    * Updated PerpetualFuturesApi.Trading.GetUserTradesAsync response model
    * Some other minor model updates
    * Updated string comparison for improved performance
    * Fixed property types on Position model

* Version 1.0.0 - 25 Mar 2024
    * Initial release