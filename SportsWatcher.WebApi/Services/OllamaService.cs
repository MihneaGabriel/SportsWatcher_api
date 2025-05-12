using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Interfaces;
using System.Text;
using System.Text.Json;

namespace SportsWatcher.WebApi.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly SportsWatcherDbContext _sportsWatcherDbcontext;

        public OllamaService(HttpClient httpClient, SportsWatcherDbContext sportsWatcherDbcontext)
        {
            _httpClient = httpClient;
            _sportsWatcherDbcontext = sportsWatcherDbcontext;
        }

        public async Task<string> InterpretJson(string jsonData)
        {
            var requestBody = new
            {
                prompt = $"Interpret this JSON data: {jsonData}.Calculate the average distance traveled per day. Find the total number of steps taken per day. Analyze the distribution of activity levels throughout the day. Identify the periods of high and low activity. Identify the activities that contributed the most to the total distance traveled. .For each task return just the value.",
                model = "gemma:2b",
                stream = false,
                format = "json"
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error calling Ollama.");
            }

            string responseString = await response.Content.ReadAsStringAsync();

            try
            {
                using JsonDocument doc = JsonDocument.Parse(responseString);
                string? extractedResponse = doc.RootElement.GetProperty("response").GetString();
                return extractedResponse?.Trim() ?? string.Empty;
            }
            catch (JsonException e)
            {
                throw new Exception("Error parsing JSON response.", e);
            }
        }

        public async Task<IActionResult> CreateAiResponse(string jsonResponse, int userId)
        {

            var aiResponse = new AiResponse
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Ollama",
                JsonResponse = jsonResponse,
                UserId = userId
            };
            try
            {
                await _sportsWatcherDbcontext.AddAsync(aiResponse);
                await _sportsWatcherDbcontext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving AI response to database: {e.Message}");
            }

            return new OkObjectResult(aiResponse);
        }
    }
}
