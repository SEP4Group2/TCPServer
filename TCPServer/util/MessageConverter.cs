using System.Globalization;
using System.Text.RegularExpressions;
using Domain.Model;
using Newtonsoft.Json;

namespace TCPServer.util;

public class MessageConverter
{
    public static PlantData CreatePlantDataFromString(string dataString)
    {
        try
        {
            
            // Define regular expressions for timestamp, TankLevel, Moisture, and UVLight
            Regex timestampRegex = new Regex(@"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})");
            Regex tankLevelRegex = new Regex(@"""TankLevel"": (\d+)");
            Regex moistureRegex = new Regex(@"""Moisture"": (\d+)");
            Regex uvLightRegex = new Regex(@"""UVLight"": (\d+)");
            Regex temperatureRegex = new Regex(@"""Temperature"":(\d+)");
            Regex humidityRegex = new Regex(@"""Humidity"":(\d+)");
            
            string timestamp = null;
            float uvLight = 0;
            float moisture = 0;
            float temperature = 0;
            float humidity = 0;
            float tanklevel = 0;

            // Read the input string line by line
            foreach (string line in dataString.Split('\n'))
            {
                // Extract temperature
                Match temperatureMatch = temperatureRegex.Match(line);
                if (temperatureMatch.Success)
                {
                    temperature = float.Parse(temperatureMatch.Groups[1].Value);
                    Console.WriteLine($"Temperature:{temperature}");
                }
                
                // Extract humidity
                Match humidityMatch = humidityRegex.Match(line);
                if (humidityMatch.Success)
                {
                    humidity = float.Parse(humidityMatch.Groups[1].Value);
                    Console.WriteLine($"Humidity:{humidity}");
                }
                
                // Extract timestamp
                Match timestampMatch = timestampRegex.Match(line);
                if (timestampMatch.Success)
                {
                     timestamp = timestampMatch.Groups[1].Value;
                    Console.WriteLine($"Timestamp: {timestamp}");
                }

                // Extract TankLevel
                Match tankLevelMatch = tankLevelRegex.Match(line);
                if (tankLevelMatch.Success)
                {
                    tanklevel = float.Parse(tankLevelMatch.Groups[1].Value);
                    Console.WriteLine($"TankLevel: {tanklevel}");
                }

                // Extract Moisture
                Match moistureMatch = moistureRegex.Match(line);
                if (moistureMatch.Success)
                {
                     moisture = float.Parse(moistureMatch.Groups[1].Value);
                    Console.WriteLine($"Moisture: {moisture}");
                }

                // Extract UVLight
                Match uvLightMatch = uvLightRegex.Match(line);
                if (uvLightMatch.Success)
                {
                    uvLight = float.Parse(uvLightMatch.Groups[1].Value);
                    Console.WriteLine($"UVLight: {uvLight}");
                }
            }

            return new PlantData()
            {
                Moisture = moisture,
                Temperature = temperature,
                UVLight = uvLight,
                TankLevel = tanklevel, 
                Humidity = humidity,
                TimeStamp = timestamp!
            };


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating PlantData: {ex.Message}");
            throw;
        }
    }
}