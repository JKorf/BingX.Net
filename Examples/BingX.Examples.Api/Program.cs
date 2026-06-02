using BingX.Net;
using BingX.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the BingX services
builder.Services.AddBingX();

// OR to provide API credentials for accessing private endpoints, or setting other options:

//builder.Services.AddBingX(options =>
//{
//    options.ApiCredentials = new BingXCredentials("<APIKEY>", "<APISECRET>");
//    options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
//});


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoints and inject the BingX rest client
app.MapGet("/{Symbol}", async ([FromServices] IBingXRestClient client, string symbol) =>
{
    var result = await client.SpotApi.ExchangeData.GetTickersAsync(symbol);
    return result.Success
        ? Results.Ok(result.Data.Single())
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.MapGet("/Balances", async ([FromServices] IBingXRestClient client) =>
{
    var result = await client.SpotApi.Account.GetBalancesAsync();
    return result.Success
        ? Results.Ok(result.Data)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.Run();
