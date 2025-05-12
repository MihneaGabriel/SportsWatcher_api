using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.Entities;
using System.Text.Json;

namespace SportsWatcher.WebApi.Interfaces
{
    public interface IOllamaService
    {
        Task<JsonDocument> InterpretJson(string jsonData);
        Task<AiResponse> CreateAiResponse(JsonDocument jsonResponse, int userId );
    }
}
