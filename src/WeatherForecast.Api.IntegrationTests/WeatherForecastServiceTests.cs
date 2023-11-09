using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Configuration;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Weasel.Core;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Services;
using WeatherForecast.Api.Services.Options;
using WeatherForecast.Api.Storage;
using WeatherForecast.Api.Storage.Entities;
using Xunit;

namespace WeatherForecast.Api.IntegrationTests
{
    [Collection("MartenDb Collection")]
    public class WeatherForecastServiceTests
    {
        private readonly IOptions<OpenMeteoOptions> _openMeteoOptions;
        public IDocumentSession DocumentSession { get; }

        public WeatherForecastServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", false)
                .Build();
            
            var store = DocumentStore.For(options =>
            {
                options.Connection(configuration.GetSection(PostgreSqlOptions.PostgreSql)[nameof(PostgreSqlOptions.ConnectionString)]);
                options.Schema.For<Coordinate>().FullTextIndex();
                options.AutoCreateSchemaObjects = AutoCreate.All;
            });
            
            _openMeteoOptions = Options.Create(configuration.GetSection(nameof(OpenMeteoOptions))
                .Get<OpenMeteoOptions>());

            DocumentSession = store.LightweightSession();
        }
        
        [Fact]
        public async Task AddForecast_SuccessCodeStatusResponse_ReturnData()
        {
            var openMeteoService = new OpenMeteoDataService(_openMeteoOptions, new PerBaseUrlFlurlClientFactory());
            var martenStorage = new MartenStorage(DocumentSession, NullLogger<IDocumentDataAccess>.Instance);
            var weatherForecastService = new Services.WeatherForecastService(openMeteoService, martenStorage);

            var weatherForecast = await weatherForecastService.AddCoordinateAsync(151.44F, 144.32F, new CancellationToken());

            weatherForecast.Should().NotBeNull();
        }
        
        [Fact]
        public async Task GetForecast_SuccessCodeStatusResponse_ReturnData()
        {
            var openMeteoService = new OpenMeteoDataService(_openMeteoOptions, new PerBaseUrlFlurlClientFactory());
            var martenStorage = new MartenStorage(DocumentSession, NullLogger<IDocumentDataAccess>.Instance);
            var weatherForecastService = new Services.WeatherForecastService(openMeteoService, martenStorage);

            var weatherForecastModel = await weatherForecastService.GetWeatherForecastAsync(151.99F, 200.03F, new CancellationToken());

            weatherForecastModel.Should().NotBeNull();
        }
        
        [Fact]
        public async Task GetForecastList_SuccessCodeStatusResponse_ReturnData()
        {
            var openMeteoService = new OpenMeteoDataService(_openMeteoOptions, new PerBaseUrlFlurlClientFactory());
            var martenStorage = new MartenStorage(DocumentSession, NullLogger<IDocumentDataAccess>.Instance);
            var weatherForecastService = new Services.WeatherForecastService(openMeteoService, martenStorage);

            var weatherForecast = await weatherForecastService.AddCoordinateAsync(100.55F, 122.14F, new CancellationToken());

            weatherForecast.Should().NotBeNull();

            var coordinates = await weatherForecastService.GetCoordinatesAsync(new CancellationToken());

            coordinates.Count().Should().NotBe(0);
        }
        
        [Fact]
        public async Task RemoveForecast_SuccessCodeStatusResponse_ReturnData()
        {
            var openMeteoService = new OpenMeteoDataService(_openMeteoOptions, new PerBaseUrlFlurlClientFactory());
            var martenStorage = new MartenStorage(DocumentSession, NullLogger<IDocumentDataAccess>.Instance);
            var weatherForecastService = new Services.WeatherForecastService(openMeteoService, martenStorage);

            var weatherForecast = await weatherForecastService.AddCoordinateAsync(88.26F, 91.05F, new CancellationToken());

            weatherForecast.Should().NotBeNull();

            await weatherForecastService.RemoveCoordinateAsync(weatherForecast.Id, new CancellationToken());
        }
    }
}