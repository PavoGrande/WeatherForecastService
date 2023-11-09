using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Services.Models;
using WeatherForecast.Api.Services.Options;

namespace WeatherForecast.Api.Services
{
    /// <summary>
    ///     Open-Meteo implementation of <see cref="IWeatherDataService"/>.
    /// </summary>
    public class OpenMeteoDataService : IWeatherDataService
    {
        private readonly OpenMeteoOptions _openMetoOptions;
        private readonly IFlurlClient _flurlClient;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenMeteoDataService"/> class.
        /// </summary>
        /// <param name="openMeteoOptions"><see cref="OpenMeteoOptions"/>OpenMeteo configuration options.</param>
        /// <param name="flurlClientFactory"><see cref="IFlurlClientFactory"/>FlurlClientFactory.</param>
        public OpenMeteoDataService(IOptions<OpenMeteoOptions> openMeteoOptions, IFlurlClientFactory flurlClientFactory)
        {
            _openMetoOptions = openMeteoOptions.Value;
            _flurlClient = flurlClientFactory.Get(openMeteoOptions.Value.Uri);
        }

        /// <inheritdoc/>
        public async Task<WeatherForecastModel> GetForecast(float longitude, float latitude, CancellationToken cancellationToken)
        {
            return await _flurlClient.Request(_openMetoOptions.Options).GetJsonAsync<WeatherForecastModel>(cancellationToken);
        }
    }
}