using Xunit;

namespace WeatherForecast.Api.IntegrationTests
{
    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}