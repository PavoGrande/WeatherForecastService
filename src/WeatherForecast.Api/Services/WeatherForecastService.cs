using AutoMapper;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Services.Mappers;
using WeatherForecast.Api.Services.Models;
using WeatherForecast.Api.Storage.Entities;

namespace WeatherForecast.Api.Services
{
    /// <summary>
    ///     WeatherForecastService API.
    /// </summary>
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherDataService _weatherDataService;
        private readonly IDocumentDataAccess _documentDataAccess;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenMeteoDataService"/> class.
        /// </summary>
        /// <param name="documentDataAccess">Open-Meteo implementation of <see cref="IDocumentDataAccess" />.</param>
        /// <param name="weatherDataService">The weather forecast service retrieval service.</param>
        public WeatherForecastService(IWeatherDataService weatherDataService, IDocumentDataAccess documentDataAccess)
        {
            _weatherDataService = weatherDataService;
            _documentDataAccess = documentDataAccess;

            var config = new MapperConfiguration(mce => mce.AddProfile(new WeatherForecastDataProfile()));
            _mapper = config.CreateMapper();
        }

        /// <summary>
        ///     Retrieves and stores weather forecast data for the provided coordinates.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task<CoordinateModel> AddCoordinateAsync(float latitude, float longitude, CancellationToken cancellationToken)
        {
            var coordinate = new Coordinate
            {
                Latitude = latitude,
                Longitude = longitude
            };

            await _documentDataAccess.AddDocumentAsync(coordinate, cancellationToken);

            return _mapper.Map<CoordinateModel>(coordinate);
        }

        /// <summary>
        ///     Retrieves a weather forecast provided the forecast Id.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task<WeatherForecastModel> GetWeatherForecastAsync(float latitude, float longitude, CancellationToken cancellationToken)
        {
            return await _weatherDataService.GetForecast(latitude, longitude, cancellationToken);
        }

        /// <summary>
        ///     Retrieves a list of coordinates.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<CoordinateModel>> GetCoordinatesAsync(CancellationToken cancellationToken)
        {
            var coordinates = await _documentDataAccess.GetDocumentsAsync<Coordinate>(cancellationToken);

            return _mapper.Map<IEnumerable<CoordinateModel>>(coordinates);
        }

        /// <summary>
        ///     Removes a weather forecast provided the forecast Id.
        /// </summary>
        /// <param name="coordinateId">The Id of the coordinates.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task RemoveCoordinateAsync(int coordinateId, CancellationToken cancellationToken)
        {
            await _documentDataAccess.DeleteDocumentAsync<Coordinate>(coordinateId, cancellationToken);
        }
    }
}