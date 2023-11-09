using AutoMapper;
using WeatherForecast.Api.Services.Models;
using WeatherForecast.Api.Storage.Entities;

namespace WeatherForecast.Api.Services.Mappers
{
    /// <summary>
    ///     Mapping Profile for WeatherForecast data.
    /// </summary>
    public class WeatherForecastDataProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastDataProfile"/> class.
        /// </summary>
        public WeatherForecastDataProfile()
        {
            CreateMap<Coordinate, CoordinateModel>();
        }
    }
}