using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        await GetWeather("https://goweather.herokuapp.com/weather/istanbul", "Istanbul");
        await GetWeather("https://goweather.herokuapp.com/weather/izmir", "Izmir");
        await GetWeather("https://goweather.herokuapp.com/weather/ankara", "Ankara");
    }

    static async Task GetWeather(string apiUrl, string cityName)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetStringAsync(apiUrl);
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(response);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nWeather in {cityName} today: {weatherData.Description}");
                Console.WriteLine($"Temperature: {weatherData.Temperature}°C");

                Console.WriteLine("\n3-Day Forecast:");

                if (weatherData.Forecast != null)
                {
                    foreach (var forecast in weatherData.Forecast)
                    {
                        Console.WriteLine($"{forecast.Day}: {forecast.Description}, Temperature: {forecast.Temperature}°C");
                    }
                }
                else
                {
                    Console.WriteLine("Forecast data not available.");
                }

                Console.ResetColor();
                Console.WriteLine("\n-------------------------------------------\n");
            }
            catch (HttpRequestException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to fetch data for {cityName}: {ex.Message}");
                Console.ResetColor();
            }
            catch (JsonException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while parsing data for {cityName}: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An unexpected error occurred for {cityName}: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}

class WeatherData
{
    public string? Description { get; set; }
    public int Temperature { get; set; }
    public Forecast[]? Forecast { get; set; }
}

class Forecast
{
    public string? Day { get; set; }
    public string? Description { get; set; }
    public int Temperature { get; set; }
}