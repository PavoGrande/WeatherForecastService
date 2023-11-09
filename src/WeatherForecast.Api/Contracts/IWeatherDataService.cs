using WeatherForecast.Api.Services.Models;

namespace WeatherForecast.Api.Contracts
{
    /// <summary>
    ///     Weather data service interface.
    /// </summary>
    public interface IWeatherDataService
    {
        /// <summary>
        ///     Retrieves weather forecast data for the provided coordinates.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed <see cref="Task"/> containing the <see cref="WeatherForecastModel"/>.</returns>
        Task<WeatherForecastModel> GetForecast(float latitude, float longitude, CancellationToken cancellationToken);
    }
}