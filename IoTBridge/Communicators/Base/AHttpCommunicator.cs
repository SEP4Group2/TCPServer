using System.Text;

namespace IoTBridge.Communicators.Base
{
    public abstract class AHttpCommunicator
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;

        protected AHttpCommunicator(string baseUrl)
        {
            this.baseUrl = baseUrl;
            httpClient = new HttpClient();
        }

        protected async Task PostAsync(string body, string endpoint)
        {
            string url = baseUrl + endpoint;
            var content = new StringContent(body, Encoding.ASCII, "application/json");

            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        
        protected async Task PostAsync(string endpoint)
        {
            string url = baseUrl + endpoint;

            var response = await httpClient.PostAsync(url, null);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        
        protected async Task UpdateAsync(string body, string endpoint)
        {
            string url = baseUrl + endpoint;
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        protected async Task<string> GetAsync(string endpoint)
        {
            string url = baseUrl + endpoint;

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
    }
}