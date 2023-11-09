using System.Text.Json.Serialization;

namespace WeatherForecast.Api.Services.Models
{
    public class CoordinateModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }
    }
}