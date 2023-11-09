using System.Diagnostics.CodeAnalysis;
using Flurl.Http.Configuration;
using Marten;
using Marten.Services;
using Microsoft.OpenApi.Models;
using Weasel.Core;
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

            services.AddOptions<PostgreSqlOptions>().Bind(configuration.GetSection(PostgreSqlOptions.PostgreSql))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var storeOptions = BuildStoreOptions(hosting, configuration);

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

        private static StoreOptions BuildStoreOptions(IHostEnvironment hosting, IConfiguration configuration)
        {
            var options = new StoreOptions();

            var serializer = new JsonNetSerializer
            {
                EnumStorage = EnumStorage.AsString
            };

            options.Connection(configuration.GetSection(PostgreSqlOptions.PostgreSql)[nameof(PostgreSqlOptions.ConnectionString)]);
            options.Serializer(serializer);

            if (hosting.IsDevelopment())
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
            else if (hosting.IsProduction())
            {
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            }

            return options;
        }
    }
}