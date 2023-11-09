using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WeatherForecast.Api.Services;
using WeatherForecast.Api.Services.Options;
using Xunit;

namespace WeatherForecast.Api.IntegrationTests
{
    public class OpenMeteoServiceTests
    {
        private readonly IOptions<OpenMeteoOptions> _openMeteoOptions;

        public OpenMeteoServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            
            _openMeteoOptions = Options.Create(configuration.GetSection(nameof(OpenMeteoOptions))
                .Get<OpenMeteoOptions>());
        }

        [Fact]
        public async Task OpenMeteoDataService_GetForecast_ReturnsData()
        {
            var openMeteoService = new OpenMeteoDataService(_openMeteoOptions, new PerBaseUrlFlurlClientFactory());

            var weatherForecastModel = await openMeteoService.GetForecast(123, 456, CancellationToken.None);

            weatherForecastModel.Should().NotBeNull();
        }
    }
}