using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;
using Flurl.Http.Configuration;
using Marten;
using Microsoft.Extensions.Options;
using WeatherForecast.Api.Contracts;
using WeatherForecast.Api.Services;
using WeatherForecast.Api.Services.Options;
using WeatherForecast.Api.Storage;

namespace WeatherForecast.Api.Extensions
{
    /// <summary>
    ///     Service collection extension methods.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds services to the services connection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1.0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            
            services.AddOptions<OpenMeteoOptions>().Bind(configuration.GetSection(nameof(OpenMeteoOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var postgreSqlOptions = Options.Create(configuration.GetSection(nameof(PostgreSqlOptions))
                .Get<PostgreSqlOptions>());

            services.AddOptions<PostgreSqlOptions>(postgreSqlOptions.Value.ConnectionString)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddMarten(opt =>
            {
                opt.Connection(postgreSqlOptions.Value.ConnectionString);
            }).UseLightweightSessions();

            services.AddScoped<IDocumentDataAccess, MartenStorage>();
            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            services.AddScoped<IWeatherDataService, OpenMeteoDataService>();
            services.AddScoped<IWeatherForecastService, Services.WeatherForecastService>();
        }
    }
}