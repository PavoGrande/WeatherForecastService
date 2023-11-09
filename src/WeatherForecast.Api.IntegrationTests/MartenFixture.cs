using System;
using System.Data;
using System.IO;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Weasel.Core;
using WeatherForecast.Api.Storage;
using WeatherForecast.Api.Storage.Entities;

namespace WeatherForecast.Api.IntegrationTests
{
    public class MartenFixture : IDisposable
    {
        private readonly ICompositeService _dockerService;

        public MartenFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", false)
                .AddJsonFile("appsettings.IntegrationTests.json", false)
                .Build();

            var dockerComposeOptions = Options.Create(configuration.GetSection(nameof(DockerComposeOptions))
                .Get<DockerComposeOptions>());

            var store = DocumentStore.For(options =>
            {
                options.Connection(configuration.GetSection(PostgreSqlOptions.PostgreSql)[nameof(PostgreSqlOptions.ConnectionString)]);
                options.Schema.For<Coordinate>().FullTextIndex();
                options.AutoCreateSchemaObjects = AutoCreate.All;
            });

            DocumentSession = store.LightweightSession();

            _dockerService = StartPostgresCompose(dockerComposeOptions, DocumentSession);
        }
        
        internal ICompositeService StartPostgresCompose(IOptions<DockerComposeOptions> dockerComposeOptions, IDocumentSession documentSession)
        {
            return new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(Path.Join(Directory.GetCurrentDirectory(), dockerComposeOptions.Value.DockerComposePath))
                .WithEnvironment(Path.Join(Directory.GetCurrentDirectory(), dockerComposeOptions.Value.DockerComposeEnvPath))
                .RemoveOrphans()
                .Wait(@"postgres", (_, _) =>
                {
                    try
                    {
                        return documentSession.Connection is { State: ConnectionState.Open } ? 0 : 5000;
                    }
                    catch (Exception)
                    {
                        return 5000;
                    }
                })
                .Build()
                .Start();
        }

        public IDocumentSession DocumentSession { get; }

        public void Dispose()
        {
            DocumentSession?.Dispose();
            _dockerService.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}