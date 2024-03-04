using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the TODO services
builder.Services.AddTODO();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddTODO(restOptions =>
{
    restOptions.ApiCredentials = new ApiCredentials("<APIKEY>", "<APISECRET>");
    restOptions.RequestTimeout = TimeSpan.FromSeconds(5);
}, socketOptions =>
{
    socketOptions.ApiCredentials = new ApiCredentials("<APIKEY>", "<APISECRET>");
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoint and inject the rest client
app.MapGet("/{Symbol}", async ([FromServices] ITODORestClient client, string symbol) =>
{
    var result = await client.SpotApi.ExchangeData.GetSpotTickersAsync(symbol);
    return result.Data.List.First().LastPrice;
})
.WithOpenApi();


app.MapGet("/Balances", async ([FromServices] ITODORestClient client) =>
{
    var result = await client.SpotApi.Account.GetBalancesAsync();
    return (object)(result.Success ? result.Data : result.Error!);
})
.WithOpenApi();

app.Run();