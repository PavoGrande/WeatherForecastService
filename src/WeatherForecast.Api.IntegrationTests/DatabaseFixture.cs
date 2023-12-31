using System;
using System.Data;
using System.IO;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WeatherForecast.Api.Storage;

namespace WeatherForecast.Api.IntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly ICompositeService _dockerService;

        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.IntegrationTests.json", false)
                .Build();

            var dockerComposeOptions = Options.Create(configuration.GetSection(nameof(DockerComposeOptions))
                .Get<DockerComposeOptions>());

            var postgreSqlOptions = Options.Create(configuration.GetSection(nameof(PostgreSqlOptions))
                .Get<PostgreSqlOptions>());

            var store = DocumentStore.For(options =>
            {
                options.Connection(postgreSqlOptions.Value.ConnectionString);
            });

            DocumentSession = store.LightweightSession();

            _dockerService = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(Path.Join(Directory.GetCurrentDirectory(), dockerComposeOptions.Value.DockerComposePath))
                .WithEnvironment(Path.Join(Directory.GetCurrentDirectory(), dockerComposeOptions.Value.DockerComposeEnvPath))
                .RemoveOrphans()
                .Wait(@"postgres", (_, _) =>
                {
                    try
                    {
                        return DocumentSession.Connection is { State: ConnectionState.Open } ? 0 : 5000;
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