namespace WeatherForecast.Api.IntegrationTests
{
    public class DockerComposeOptions
    {
        private const string DefaultDockerComposePath = @"..\..\docker-compose.yml";

        private const string DefaultDockerComposeEnvPath = @"..\..\.env";

        public DockerComposeOptions()
        {
            DockerComposePath = DefaultDockerComposePath;
            DockerComposeEnvPath = DefaultDockerComposeEnvPath;
        }

        public string DockerComposePath { get; set; }

        public string DockerComposeEnvPath { get; set; }
    }
}