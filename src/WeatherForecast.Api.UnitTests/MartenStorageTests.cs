using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Marten;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Npgsql;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Storage;
using WeatherForecast.Api.Storage.Entities;
using Xunit;

namespace WeatherForecast.Api.UnitTests
{
    public class MartenStorageTests
    {
        [Fact]
        public async Task MartenStorage_AddDocumentAsync_ExpectSuccess()
        {
            var coordinate = new Coordinate
            {
                Latitude = 55.255F,
                Longitude = 88.788F
            };

            var cancellationToken = new CancellationToken();
            var documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);

            documentSessionMock.Setup(x => x.Insert(coordinate));
            documentSessionMock.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns<CancellationToken>(_ => Task.CompletedTask);

            var martenStorage = new MartenStorage(documentSessionMock.Object, NullLogger<IDocumentDataAccess>.Instance);
            await martenStorage.AddDocumentAsync(coordinate, cancellationToken);

            documentSessionMock.Verify(x => x.Insert(coordinate), Times.Once);
            documentSessionMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            documentSessionMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task MartenStorage_AddDocumentAsync_ExpectDocumentDbException_UnknownErrorMessage()
        {
            var coordinate = new Coordinate
            {
                Latitude = 55.255F,
                Longitude = 88.788F
            };

            var cancellationToken = new CancellationToken();
            var documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);

            documentSessionMock.Setup(x => x.Insert(coordinate));
            documentSessionMock.Setup(x => x.SaveChangesAsync(cancellationToken))
                .ThrowsAsync(new NpgsqlException());

            var martenStorage = new MartenStorage(documentSessionMock.Object, NullLogger<IDocumentDataAccess>.Instance);

            var documentDbException = await Assert.ThrowsAsync<DocumentDataAccessException>(async () =>
                await martenStorage.AddDocumentAsync(coordinate, cancellationToken));

            documentDbException.Message.Should().Be(DocumentDataAccessConstants.UnHandledException);
        }

        [Fact]
        public async Task MartenStorage_GetDocumentAsync_ExpectSuccess()
        {
            var coordinate = new Coordinate
            {
                Latitude = 55.255F,
                Longitude = 88.788F
            };

            var cancellationToken = new CancellationToken();
            var documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);

            documentSessionMock.Setup(x => x.LoadAsync<Coordinate>(coordinate.Id, cancellationToken))
                .ReturnsAsync(coordinate);

            var martenStorage = new MartenStorage(documentSessionMock.Object, NullLogger<IDocumentDataAccess>.Instance);
            var document = await martenStorage.GetDocumentAsync<Coordinate>(coordinate.Id, cancellationToken);

            document.Should().NotBeNull();
            coordinate.Id.Should().Be(document.Id);
            documentSessionMock.Verify(x => x.LoadAsync<Coordinate>(coordinate.Id, cancellationToken), Times.Once);
            documentSessionMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task MartenStorage_GetDocumentAsync_ExpectDocumentDbException_UnknownErrorMessage()
        {
            var coordinateId = 1;
            var cancellationToken = new CancellationToken();
            var documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);

            documentSessionMock.Setup(x => x.LoadAsync<Coordinate>(coordinateId, cancellationToken))
                .ThrowsAsync(new NpgsqlException());

            var martenStorage = new MartenStorage(documentSessionMock.Object, NullLogger<IDocumentDataAccess>.Instance);

            var documentDbException = await Assert.ThrowsAsync<DocumentDataAccessException>(async () =>
                await martenStorage.GetDocumentAsync<Coordinate>(coordinateId, cancellationToken));

            documentDbException.Should().NotBeNull();
            documentDbException.Message.Should().Be(DocumentDataAccessConstants.UnHandledException);
        }

        [Fact]
        public async Task MartenStorage_DeleteDocumentAsync_ExpectSuccess()
        {
            var coordinate = new Coordinate
            {
                Latitude = 55.255F,
                Longitude = 88.788F
            };

            var cancellationToken = new CancellationToken();
            var documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);

            documentSessionMock.Setup(x => x.Delete<Coordinate>(coordinate.Id));
            documentSessionMock.Setup(x => x.SaveChangesAsync(cancellationToken)).Returns<CancellationToken>(_ => Task.CompletedTask);

            var martenStorage = new MartenStorage(documentSessionMock.Object, NullLogger<IDocumentDataAccess>.Instance);
            await martenStorage.DeleteDocumentAsync<Coordinate>(coordinate.Id, cancellationToken);

            documentSessionMock.Verify(x => x.Delete<Coordinate>(coordinate.Id), Times.Once);
            documentSessionMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
            documentSessionMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task MartenStorage_DeleteDocumentAsync_ExpectDocumentDbException_UnknownErrorMessage()
        {
            var coordinateId = 1;
            var cancellationToken = new CancellationToken();
            var documentSessionMock = new Mock<IDocumentSession>(MockBehavior.Strict);

            documentSessionMock.Setup(x => x.Delete<Coordinate>(coordinateId))
                .Throws(new NpgsqlException());

            var martenStorage = new MartenStorage(documentSessionMock.Object, NullLogger<IDocumentDataAccess>.Instance);

            var documentDbException = await Assert.ThrowsAsync<DocumentDataAccessException>(async () =>
                await martenStorage.DeleteDocumentAsync<Coordinate>(coordinateId, cancellationToken));

            documentDbException.Should().NotBeNull();
            documentDbException.Message.Should().Be(DocumentDataAccessConstants.UnHandledException);
        }
    }
}