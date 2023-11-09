using Asp.Versioning.Conventions;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Extensions;
using WeatherForecast.Api.Services.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion( 1.0 )
    .ReportApiVersions()
    .Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("v{version:apiVersion}/coordinate", async ([FromBody]CoordinateModel coordinateModel, IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.AddCoordinateAsync(coordinateModel.Latitude, coordinateModel.Longitude,
        new CancellationToken()))
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

app.MapGet("v{version:apiVersion}/forecast", async ([FromBody]CoordinateModel coordinateModel, IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.GetWeatherForecastAsync(coordinateModel.Latitude, coordinateModel.Longitude,
        new CancellationToken()))
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

app.MapGet("v{version:apiVersion}/coordinates", async (IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.GetCoordinatesAsync(new CancellationToken()))
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

app.MapDelete("v{version:apiVersion}/coordinate/{coordinateId}", async (int coordinateId, IWeatherForecastService weatherForecastService) => 
    await weatherForecastService.RemoveCoordinateAsync(coordinateId, new CancellationToken()))
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1.0);

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (FlurlHttpException ex)
    {
        context.Response.StatusCode = ex.StatusCode!.Value;
        await context.Response.WriteAsync("Http error occurred.");
    }
    catch (DocumentDataAccessException ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Database error occurred.");
    }
});


app.Run();