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

            return aiResponses;
        }

        public async Task<JsonDocument> InterpretJson(string jsonData, int categoryId)
        {
            var requestBody = new
            {
                prompt = getPromptBasedOnCategory(jsonData, categoryId),
                model = "gemma:2b",
                stream = false,
                format = "json",
                temperature = 0.2
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
                5 => $@"You are a performance coach analyzing a running workout based on the provided data. Use only metric units: meters for distance, seconds for time, beats per minute (bpm) for heart rate, and steps per minute for cadence. Input Data: {{jsonData}} Your tasks: 1. Calculate the total distance (meters) and average pace (seconds per kilometer). 2. Identify the total duration, including elapsed time and moving time (seconds). 3. Calculate the average heart rate (bpm) and average cadence (steps per minute). 4. Detect intervals or splits by analyzing pace variation over time, returning an array of splits with start/end times (seconds) and pace (seconds per kilometer). 5. Highlight warm-up, peak, and cool-down segments with their respective start/end times (seconds) and pace. 6. Evaluate terrain impact using elevation data, calculating total elevation gain (meters) and average gradient (percentage). 7. Summarize performance trends, such as acceleration, fatigue, and recovery patterns, with clear numeric indicators. 8. Provide actionable suggestions to improve future running sessions based on the analysis. 9. Return only the results in a structured JSON object using the exact keys: ""total_distance"", ""average_pace"", ""total_duration"", ""average_heart_rate"", ""average_cadence"", ""splits"", ""warm_up"", ""peak"", ""cool_down"", ""terrain_impact"", ""performance_trends"", and ""suggestions"". Do NOT include any commentary or explanation outside the JSON. Example output format: {{ ""total_distance"": ""number (meters)"", ""average_pace"": ""number (seconds per kilometer)"", ""total_duration"": ""number (seconds)"", ""average_heart_rate"": ""number (bpm)"", ""average_cadence"": ""number (steps per minute)"", ""splits"": [ {{""start_time"": ""number (seconds)"", ""end_time"": ""number (seconds)"", ""pace"": ""number (seconds per kilometer)""}}, {{""start_time"": ""number (seconds)"", ""end_time"": ""number (seconds)"", ""pace"": ""number (seconds per kilometer)""}} ], ""warm_up"": {{""start_time"": ""number (seconds)"", ""end_time"": ""number (seconds)"", ""pace"": ""number (seconds per kilometer)""}}, ""peak"": {{""start_time"": ""number (seconds)"", ""end_time"": ""number (seconds)"", ""pace"": ""number (seconds per kilometer)""}}, ""cool_down"": {{""start_time"": ""number (seconds)"", ""end_time"": ""number (seconds)"", ""pace"": ""number (seconds per kilometer)""}}, ""terrain_impact"": {{ ""elevation_gain"": ""number (meters)"", ""average_gradient"": ""number (percentage)"" }}, ""performance_trends"": {{ ""acceleration"": ""number (arbitrary units)"", ""fatigue"": ""number (arbitrary units)"", ""recovery"": ""number (arbitrary units)"" }}, ""suggestions"": [ ""string (suggestion 1)"", ""string (suggestion 2)"", ""string (suggestion 3)"" ] }}",                
                6 => $@"You are a swim coach reviewing a pool training session. Data: {jsonData} Your tasks: 1. Estimate the total swim distance (in meters) and total time (in seconds). 2. Determine the average and maximum pace (in seconds per 100 meters). 3. Analyze heart rate trends (in beats per minute) throughout the session. 4. Identify rest periods or pauses (with durations in seconds). 5. Detect stroke consistency based on cadence or motion frequency (strokes per minute). 6. Summarize performance across the session (start, middle, end). 7. Suggest possible technique improvements based on speed and effort patterns. Return the findings strictly as JSON in the following structure: {{ ""total_distance"": number, ""total_time"": number, ""average_pace"": string, ""best_pace"": string, ""heart_rate_trends"": [ {{ ""heart_rate"": number, ""time_point"": number }} ], ""rest_periods"": [ {{ ""duration"": number, ""start_time"": number, ""end_time"": number }} ], ""stroke_consistency"": {{ ""average_cadence"": number, ""variability"": number }}, ""performance_summary"": {{ ""start"": string, ""middle"": string, ""end"": string }}, ""technique_suggestions"": [ string ] }} Use only metric units (meters, seconds, bpm). Do not include any extra commentary — just the JSON object. ",
                7 => $@"You are an expert sports data analyst specializing in cycling performance. Analyze the following cycling activity data provided in JSON format. Data: {jsonData} Your tasks: 1. Calculate total distance traveled (in kilometers). 2. Calculate total moving time and total elapsed time (in minutes). 3. Determine average speed and maximum speed (in km/h). 4. Calculate average power output (in watts). 5. Identify the highest altitude gain from climbs (in meters). 6. Analyze average cadence (rpm) and average heart rate (bpm). 7. Highlight periods of high effort based on both power output and cadence. Include multiple periods if applicable. 8. Identify segments with the steepest gradients where a noticeable speed drop occurred. Include start and end times (in minutes), grade (%), and speed drop (km/h). 9. Provide 3 suggestions for performance improvement, recovery, or training strategy based on the analysis. Return only the results in a structured JSON object using the exact keys shown below. Do NOT include any commentary or explanation outside the JSON. Expected JSON format: {{ ""total_distance"": float, ""total_moving_time"": float, ""average_speed"": float, ""average_max_speed"": float, ""average_power_output"": float, ""highest_climb_altitude_gain"": float, ""average_cadence"": float, ""average_heart_rate"": float, ""high_effort_periods"": [ {{ ""power_output"": float, ""cadence"": float }} ], ""segments_with_steepest_grades_and_speed_drop"": [ {{ ""start_time"": float, ""end_time"": float, ""grade"": float, ""speed_drop"": float }} ], ""suggestions"": [ ""string"", ""string"", ""string"" ] }}",
                _ => $"You are a fitness data analyst. Analyze the following activity data from a workout session.\r\n\r\nData: {jsonData}\r\n\r\nTasks:\r\n1. Estimate total distance, duration (moving and elapsed), and average pace or speed.\r\n2. Detect changes in effort, pace, or heart rate throughout the session.\r\n3. Highlight any significant activity segments such as sprints, climbs, rests, or intervals.\r\n4. Analyze cadence, heart rate, elevation, and power if available.\r\n5. Identify patterns of fatigue, recovery, or high performance.\r\n6. Summarize this workout in 3 bullet points and provide a detailed breakdown in JSON format.\r\n\r\nReturn:\r\n- A short summary.\r\n- A JSON with keys like \"distance\", \"average_speed\", \"effort_peaks\", \"fatigue_pattern\", etc.\r\n"
            };
        }

    }
}
