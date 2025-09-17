using MauiAppTempoAgora.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace MauiAppTempoAgora.Services
{
    public static class DataService
    {
        private static readonly string apiKey = "6135072afe7f6cec1537d5cb08a5a1a2";
        private static readonly string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            using HttpClient client = new HttpClient();
            string url = $"{baseUrl}?q={cidade}&appid={apiKey}&units=metric&lang=pt_br";

            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);

            Tempo t = new Tempo();

            // Coord
            if (doc.RootElement.TryGetProperty("coord", out JsonElement coord))
            {
                t.lon = coord.GetProperty("lon").GetDouble();
                t.lat = coord.GetProperty("lat").GetDouble();
            }

            // Main
            if (doc.RootElement.TryGetProperty("main", out JsonElement main))
            {
                t.temp_min = main.GetProperty("temp_min").GetDouble();
                t.temp_max = main.GetProperty("temp_max").GetDouble();
            }

            // Weather (array)
            if (doc.RootElement.TryGetProperty("weather", out JsonElement weatherArray) && weatherArray.GetArrayLength() > 0)
            {
                var weather = weatherArray[0];
                t.main = weather.GetProperty("main").GetString();
                t.description = weather.GetProperty("description").GetString();
            }

            // Wind
            if (doc.RootElement.TryGetProperty("wind", out JsonElement wind))
            {
                t.speed = wind.GetProperty("speed").GetDouble();
            }

            // Visibility
            if (doc.RootElement.TryGetProperty("visibility", out JsonElement visibility))
            {
                t.visibility = visibility.GetInt32();
            }

            // Sys (sunrise/sunset)
            if (doc.RootElement.TryGetProperty("sys", out JsonElement sys))
            {
                long sunriseUnix = sys.GetProperty("sunrise").GetInt64();
                long sunsetUnix = sys.GetProperty("sunset").GetInt64();

                t.sunrise = DateTimeOffset.FromUnixTimeSeconds(sunriseUnix).ToLocalTime().ToString("HH:mm");
                t.sunset = DateTimeOffset.FromUnixTimeSeconds(sunsetUnix).ToLocalTime().ToString("HH:mm");
            }

            return t;
        }
    }
}

