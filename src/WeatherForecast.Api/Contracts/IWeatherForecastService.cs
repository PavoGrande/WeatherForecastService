using WeatherForecast.Api.Services.Models;

namespace WeatherForecast.Api.Contracts
{
    /// <summary>
    ///     Interface for WeatherForecastService.
    /// </summary>
    public interface IWeatherForecastService
    {
        /// <summary>
        ///     Adds a coordinate to storage.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A completed <see cref="Task"/> containing the <see cref="CoordinateModel"/>.</returns>
        Task<CoordinateModel> AddCoordinateAsync(float latitude, float longitude, CancellationToken cancellationToken);

        /// <summary>
        ///     Retrieves a weather forecast for the provided coordinates.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A completed <see cref="Task"/> containing the <see cref="WeatherForecastModel"/>.</returns>
        Task<WeatherForecastModel> GetWeatherForecastAsync(float latitude, float longitude,
            CancellationToken cancellationToken);

        /// <summary>
        ///     Retrieves a list of coordinates.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A completed <see cref="Task"/> containing a list of <see cref="CoordinateModel"/>.</returns>
        Task<IEnumerable<CoordinateModel>> GetCoordinatesAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Removes a coordinate from storage.
        /// </summary>
        /// <param name="coordinateId">The Id of the coordinates.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task RemoveCoordinateAsync(int coordinateId, CancellationToken cancellationToken);
    }
}