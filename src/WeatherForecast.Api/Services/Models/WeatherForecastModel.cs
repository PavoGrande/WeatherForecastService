using System.Text.Json.Serialization;

namespace WeatherForecast.Api.Services.Models
{
    public class WeatherForecastModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }

        [JsonPropertyName("elevation")]
        public float Elevation { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public float GenerationTimeMs { get; set; }

        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("hourly")]
        public HourlyData Hourly { get; set; }
    }

    public class HourlyData
    {
        [JsonPropertyName("time")]
        public IEnumerable<DateTime> Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public IEnumerable<float> Temperature2m { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public IEnumerable<int> RelativeHumidity2m { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public IEnumerable<float> WindSpeed10m { get; set; }
    }
}