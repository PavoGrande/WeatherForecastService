using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Services.Models;
using WeatherForecast.Api.Storage;
using WeatherForecast.Api.Storage.Entities;
using Xunit;

namespace WeatherForecast.Api.IntegrationTests
{
    [Collection("MartenDb Collection")]
    public class MartenStorageTests
    {
        private readonly MartenFixture _martenFixture;

        public MartenStorageTests(MartenFixture martenFixture)
        {
            _martenFixture = martenFixture;
        }

        [Fact]
        public async Task MartenStorage_AddDocumentAsync_ExpectSuccessResponse_Test()
        {
            var coordinate = new Coordinate
            {
                Latitude = 99.899F,
                Longitude = 100.88F
            };

            var martenStorage = new MartenStorage(_martenFixture.DocumentSession, NullLogger<IDocumentDataAccess>.Instance);

            await martenStorage.AddDocumentAsync(coordinate, new CancellationToken());
        }

        [Fact]
        public async Task MartenStorage_GetDocumentAsync_ExpectSuccessResponse_Test()
        {
            var coordinate = new Coordinate
            {
                Latitude = 99.899F,
                Longitude = 100.88F
            };

            var martenStorage = new MartenStorage(_martenFixture.DocumentSession, NullLogger<IDocumentDataAccess>.Instance);

            await martenStorage.AddDocumentAsync(coordinate, new CancellationToken());

            var result = await martenStorage.GetDocumentAsync<Coordinate>(coordinate.Id, CancellationToken.None);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task MartenStorage_GetDocumentsAsync_ExpectSuccessResponse_Test()
        {
            var coordinate = new Coordinate
            {
                Latitude = 99.899F,
                Longitude = 100.88F
            };

            var martenStorage = new MartenStorage(_martenFixture.DocumentSession, NullLogger<IDocumentDataAccess>.Instance);

            await martenStorage.AddDocumentAsync(coordinate, new CancellationToken());

            var result = await martenStorage.GetDocumentsAsync<Coordinate>(new CancellationToken());

            result.Should().NotBeNull();
            result.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task MartenStorage_DeleteDocumentAsync_ExpectSuccessResponse_Test()
        {
            var coordinate = new Coordinate
            {
                Id = 100,
                Latitude = 99.899F,
                Longitude = 100.88F
            };

            var martenStorage = new MartenStorage(_martenFixture.DocumentSession, NullLogger<IDocumentDataAccess>.Instance);

            await martenStorage.AddDocumentAsync(coordinate, new CancellationToken());

            await martenStorage.DeleteDocumentAsync<CoordinateModel>(coordinate.Id, CancellationToken.None);

            var result = await martenStorage.GetDocumentAsync<CoordinateModel>(coordinate.Id, CancellationToken.None);

            result.Should().BeNull();
        }
    }
}