using Xunit;

namespace WeatherForecast.Api.IntegrationTests
{
    [CollectionDefinition("MartenDb Collection")]
    public class MartenCollection : ICollectionFixture<MartenFixture>
    {
    }
}