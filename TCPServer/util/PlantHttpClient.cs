using System.Text;
using System.Text.Json;
using Domain.Model;

namespace TCPServer.util;

public class PlantHttpClient
{
    public async Task SavePlantDataAsync(PlantData newPlantData)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Replace the base URL with the actual base URL of your API
                string baseUrl = "http://plantapi:5000"; // Example base URL
                string endpoint = "/PlantData/savePlantData";

                // Concatenate the base URL and endpoint
                string apiUrl = $"{baseUrl}{endpoint}";

                string jsonPlantData = JsonSerializer.Serialize(newPlantData);
                var content = new StringContent(jsonPlantData, Encoding.UTF8, "application/json");

                // Send a POST request
                var response = await client.PostAsync(apiUrl, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Plant data saved successfully: {responseData}");
                }
                else
                {
                    // Handle error response
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
    
    
    
}