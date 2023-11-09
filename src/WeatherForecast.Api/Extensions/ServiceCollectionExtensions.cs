using System.Diagnostics.CodeAnalysis;
using Flurl.Http.Configuration;
using Marten;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
        /// <param name="hosting"><see cref="IHostEnvironment"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public static void AddServices(this IServiceCollection services, IHostEnvironment hosting, IConfiguration configuration)
        {
            services.AddOptions<OpenMeteoOptions>().Bind(configuration.GetSection(nameof(OpenMeteoOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var postgreSqlOptions = Options.Create(configuration.GetSection(nameof(PostgreSqlOptions))
                .Get<PostgreSqlOptions>());

            services.AddOptions<PostgreSqlOptions>(postgreSqlOptions.Value.ConnectionString)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var storeOptions = new StoreOptions();
            storeOptions.Connection(postgreSqlOptions.Value.ConnectionString);

            services.AddMarten(opt => new StoreOptions().Connection(""));
    
            services.AddMarten(storeOptions).UseLightweightSessions();

            services.AddScoped<IDocumentDataAccess, MartenStorage>();
            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            services.AddScoped<IWeatherDataService, OpenMeteoDataService>();
            services.AddScoped<IWeatherForecastService, Services.WeatherForecastService>();

            services.AddApiVersioning();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherForecastService.Runner", Version = "v1" });
            });
        }
    }
}