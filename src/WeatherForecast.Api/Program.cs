using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Extensions;
using WeatherForecast.Api.Services.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(new HostingEnvironment(), builder.Configuration);

var app = builder.Build();

app.MapPost("/coordinate", async ([FromBody]CoordinateModel coordinateModel, IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.AddCoordinateAsync(coordinateModel.Latitude, coordinateModel.Longitude,
        new CancellationToken()));

app.MapGet("/forecast", async ([FromBody]CoordinateModel coordinateModel, IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.GetWeatherForecastAsync(coordinateModel.Latitude, coordinateModel.Longitude,
        new CancellationToken()));

app.MapGet("/coordinates", async (IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.GetCoordinatesAsync(new CancellationToken()));

app.MapDelete("/coordinate/{coordinateId}", async (int coordinateId, IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.RemoveCoordinateAsync(coordinateId, new CancellationToken()));

app.Run();