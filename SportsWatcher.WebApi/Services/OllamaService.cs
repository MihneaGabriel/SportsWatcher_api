using Microsoft.EntityFrameworkCore;
using SportsWatcher.WebApi.DTOs;
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

        public async Task<List<AiResponse>> GetAiResponsesAsync(int userId, int categoryId)
        {
            var aiResponses = await _sportsWatcherDbcontext.AiResponse
                .Where(r => r.UserId == userId && r.CategoryId == categoryId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            if(aiResponses == null || aiResponses.Count == 0)
            {
                throw new Exception("No AI processes found for the given user and category.");
            }

            return aiResponses;
        }

        public async Task<JsonDocument> InterpretJson(string jsonData, int categoryId)
        {
            var requestBody = new
            {
                prompt = getPromptBasedOnCategory(jsonData, categoryId),
                model = "gemma:2b",
                stream = false,
                format = "json"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error calling Ollama.");
            }

            string responseString = await response.Content.ReadAsStringAsync();

            try
            {
                using JsonDocument fullResponse = JsonDocument.Parse(responseString);

                // Ollama wraps the real response as a string in the `response` property
                string rawJson = fullResponse.RootElement.GetProperty("response").GetString();

                if (string.IsNullOrWhiteSpace(rawJson))
                    throw new Exception("Empty response received from Ollama.");

                return JsonDocument.Parse(rawJson);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to parse JSON from Ollama response.", ex);
            }
        }

        public async Task<AiResponse> CreateAiResponse(JsonDocument jsonResponse, AiResponseDto aiResponseDto)
        {

            var aiResponse = new AiResponse
            {
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "Ollama",
                JsonResponse = jsonResponse.RootElement.GetRawText(),
                UserId = aiResponseDto.UserId,
                CategoryId = aiResponseDto.CategoryId
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

            return aiResponse;
        }

        private string getPromptBasedOnCategory(string jsonData, int categoryId)
        {
            return categoryId switch
            {
                // Be careful if the id's are changed
                5 => $"You are a performance coach analyzing a running workout.\r\n\r\nData: {jsonData}\r\n\r\nYour tasks:\r\n1. Calculate total distance and average pace (min/km).\r\n2. Identify total duration (elapsed and moving time).\r\n3. Calculate average heart rate and cadence.\r\n4. Detect intervals or splits based on pace variation.\r\n5. Highlight warm-up, peak, and cool-down segments.\r\n6. Evaluate terrain impact using elevation data.\r\n7. Summarize performance trends like acceleration, fatigue, or recovery patterns.\r\n8. Return the results in structured JSON with clear keys like \"average_pace\", \"splits\", etc.\r\n",
                6 => $"You are a swim coach reviewing a pool training session.\r\n\r\nData: {jsonData}\r\n\r\nYour tasks:\r\n1. Estimate total swim distance and total time.\r\n2. Determine average and max pace (per 100m if possible).\r\n3. Analyze heart rate trends throughout the session.\r\n4. Identify rest periods or pauses.\r\n5. Detect stroke consistency based on cadence or motion frequency.\r\n6. Summarize performance across the session (start, middle, end).\r\n7. Suggest possible technique improvements based on speed/effort patterns.\r\n8. Return your findings in a well-structured JSON format.\r\n",
                7 => $"You are an expert sports data analyst. Analyze the following cycling activity data.\r\n\r\nData: {jsonData}\r\n\r\nYour tasks:\r\n1. Calculate total distance traveled (in km).\r\n2. Calculate total moving time and total elapsed time (in minutes).\r\n3. Determine the average and maximum speed (km/h).\r\n4. Calculate average power output (watts).\r\n5. Identify the highest climb or altitude gain (meters).\r\n6. Analyze average cadence and heart rate.\r\n7. Highlight periods of high effort based on power and cadence.\r\n8. Identify segments with steepest grades and speed drop.\r\n9. Return a JSON object with clear keys and values for each item above.\r\n",
                _ => $"You are a fitness data analyst. Analyze the following activity data from a workout session.\r\n\r\nData: {jsonData}\r\n\r\nTasks:\r\n1. Estimate total distance, duration (moving and elapsed), and average pace or speed.\r\n2. Detect changes in effort, pace, or heart rate throughout the session.\r\n3. Highlight any significant activity segments such as sprints, climbs, rests, or intervals.\r\n4. Analyze cadence, heart rate, elevation, and power if available.\r\n5. Identify patterns of fatigue, recovery, or high performance.\r\n6. Summarize this workout in 3 bullet points and provide a detailed breakdown in JSON format.\r\n\r\nReturn:\r\n- A short summary.\r\n- A JSON with keys like \"distance\", \"average_speed\", \"effort_peaks\", \"fatigue_pattern\", etc.\r\n"
            };
        }

    }
}
