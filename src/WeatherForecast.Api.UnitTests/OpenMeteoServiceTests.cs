using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using WeatherForecast.Api.Services;
using WeatherForecast.Api.Services.Options;
using Xunit;

namespace WeatherForecast.Api.UnitTests
{
    public class OpenMeteoServiceTests
    {
        [Fact]
        public async Task GetForecast_SuccessCodeStatusResponse_ReturnData()
        {
            using var httpTest = new HttpTest();
            var openMeteoOptions = Options.Create(new OpenMeteoOptions
            {
                Uri = "http://127.0.0.1"
            });

            var openMeteoService = new OpenMeteoDataService(openMeteoOptions, new PerBaseUrlFlurlClientFactory());

            var forcastModel = await openMeteoService.GetForecast(123, 456, CancellationToken.None);
        }
    }
}