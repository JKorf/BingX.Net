using BingX.Net.Clients;

// REST
var restClient = new BingXRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetSpotTickersAsync("ETH-USDT");
Console.WriteLine($"Rest client ticker price for ETH-USDT: {ticker.Data.List.First().LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new BingXSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH-USDT", update =>
{
    Console.WriteLine($"Websocket client ticker price for ETHUSDT: {update.Data.LastPrice}");
});

Console.ReadLine();
