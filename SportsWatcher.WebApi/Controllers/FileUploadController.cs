using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Entities;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ICsvParserService _csvParserService;
        private readonly IOllamaService _ollamaService;

        public FileUploadController(ICsvParserService csvParserService, IOllamaService ollamaService)
        {
            _csvParserService = csvParserService;
            _ollamaService = ollamaService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv([FromForm] AiResponseDto aiResponse)
        {
            if (aiResponse.File == null || aiResponse.File.Length == 0)
            {
                return BadRequest(new { message = "Invalid file. Please upload a non-empty CSV file." });
            }

            if (aiResponse.UserId <= 0) 
            {
                return BadRequest(new { message = "The user doesn't exist" });
            }

            using var stream = new MemoryStream();

            await aiResponse.File.CopyToAsync(stream);

            stream.Position = 0;

            // Parse the CSV file to JSON
            string jsonData = _csvParserService.ParseCsvToJson(stream);

            // Send the JSON data to the Ollama service for interpretation
            string ollamaResponse = await _ollamaService.InterpretJson(jsonData);

            // Save the parsed data to the database
            await _ollamaService.CreateAiResponse(ollamaResponse, aiResponse.UserId );

            return Ok(new AiResponse { JsonResponse = ollamaResponse });
        }
    }
}




