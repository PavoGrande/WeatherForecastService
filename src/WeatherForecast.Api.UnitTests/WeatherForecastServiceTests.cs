using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Services;
using WeatherForecast.Api.Services.Models;
using WeatherForecast.Api.Storage.Entities;
using Xunit;

namespace WeatherForecast.Api.UnitTests
{
    public class WeatherForecastServiceTests
    {
        [Fact]
        public async Task WeatherForecastService_AddCoordinateAsync_ReturnsSuccess()
        {
            var weatherDataServiceMock = new Mock<IWeatherDataService>();
            var documentDataAccessMock = new Mock<IDocumentDataAccess>();

            var coordinate = new Coordinate
            {
                Latitude = 22.11F,
                Longitude = 33.44F
            };

            var weatherForecastService =
                new WeatherForecastService(weatherDataServiceMock.Object, documentDataAccessMock.Object);

            var result = await weatherForecastService.AddCoordinateAsync(coordinate.Latitude, coordinate.Longitude,
                    CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeOfType<CoordinateModel>();
        }

        [Fact]
        public async Task WeatherForecastService_GetWeatherForecastAsync_ReturnsSuccess()
        {
            var weatherDataServiceMock = new Mock<IWeatherDataService>();
            var documentDataAccessMock = new Mock<IDocumentDataAccess>();

            var coordinate = new Coordinate
            {
                Latitude = 22.11F,
                Longitude = 33.44F
            };

            var weatherForecastModel = new WeatherForecastModel
            {
                Latitude = 22.11F,
                Longitude = 33.44F,
                Elevation = 150.00F,
                GenerationTimeMs = 00.01F,
                Hourly = new HourlyData
                {
                    RelativeHumidity2m = new List<int> { 1, 2, 5, 9, 99 },
                    Temperature2m = new List<float> { 11.11F, 33.33F, 99, 88F },
                    Time = new List<DateTime> {DateTime.Now, DateTime.Now.AddHours(1), DateTime.Now.AddHours(2) },
                    WindSpeed10m = new List<float> { 10.44F, 7.32F, 9.08F }
                }
            };

            weatherDataServiceMock
                .Setup(x => x.GetForecast(coordinate.Latitude, coordinate.Longitude, CancellationToken.None))
                .ReturnsAsync(weatherForecastModel);

            var weatherForecastService =
                new WeatherForecastService(weatherDataServiceMock.Object, documentDataAccessMock.Object);

            var result = await weatherForecastService.GetWeatherForecastAsync(coordinate.Latitude, coordinate.Longitude,
                CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeOfType<WeatherForecastModel>();
            result.Latitude.Should().Be(weatherForecastModel.Latitude);
            result.Longitude.Should().Be(weatherForecastModel.Longitude);
            result.Elevation.Should().Be(weatherForecastModel.Elevation);
            result.GenerationTimeMs.Should().Be(weatherForecastModel.GenerationTimeMs);
            result.Hourly.Should().Be(weatherForecastModel.Hourly);
        }

        [Fact]
        public async Task WeatherForecastService_GetCoordinatesAsync_ReturnsSuccess()
        {
            var weatherDataServiceMock = new Mock<IWeatherDataService>();
            var documentDataAccessMock = new Mock<IDocumentDataAccess>();

            var coordinates = new List<Coordinate>
            {
                new ()
                {
                    Latitude = 22.11F,
                    Longitude = 33.44F
                },
                new ()
                {
                    Latitude = 22.11F,
                    Longitude = 33.44F
                },
                new ()
                {
                    Latitude = 22.11F,
                    Longitude = 33.44F
                }
            };

            var weatherForecastService =
                new WeatherForecastService(weatherDataServiceMock.Object, documentDataAccessMock.Object);

            documentDataAccessMock.Setup(x => x.GetDocumentsAsync<Coordinate>(CancellationToken.None))
                .ReturnsAsync(coordinates);

            var result = (await weatherForecastService.GetCoordinatesAsync(CancellationToken.None)).ToList();

            result.Should().NotBeNull();
            result.Should().BeOfType<List<CoordinateModel>>();
            result.Count().Should().Be(3);
        }

        [Fact]
        public async Task WeatherForecastService_RemoveCoordinateAsync_ReturnsSuccess()
        {
            var weatherDataServiceMock = new Mock<IWeatherDataService>();
            var documentDataAccessMock = new Mock<IDocumentDataAccess>();

            var weatherForecastService =
                new WeatherForecastService(weatherDataServiceMock.Object, documentDataAccessMock.Object);

            await weatherForecastService.RemoveCoordinateAsync(1, CancellationToken.None);
        }
    }
}