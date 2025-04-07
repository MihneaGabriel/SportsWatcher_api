using Microsoft.AspNetCore.Mvc;

namespace SportsWatcher.WebApi.Interfaces
{
    public interface IOllamaService
    {
        Task<string> InterpretJson(string jsonData);
        Task<IActionResult> CreateAiResponse(string jsonResponse);
    }
}
