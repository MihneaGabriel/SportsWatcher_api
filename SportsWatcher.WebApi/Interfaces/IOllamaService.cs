using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;
using System.Text.Json;

namespace SportsWatcher.WebApi.Interfaces
{
    public interface IOllamaService
    {
        Task<List<AiResponse>> GetAiResponsesAsync(int userId, int categoryId);
        Task<JsonDocument> InterpretJson(string jsonData, int categoryId);
        Task<AiResponse> CreateAiResponse(JsonDocument jsonResponse, AiResponseDto aiResponseDto);
    }
}
