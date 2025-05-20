using Microsoft.AspNetCore.Mvc;
using SportsWatcher.WebApi.DTOs;
using SportsWatcher.WebApi.Interfaces;
using System.Text.Json;

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

        [HttpGet("GetAiResponse")]
        public async Task<IActionResult> GetAiResponse([FromQuery] int userId, [FromQuery] int categoryId)
        {
            if (userId <= 0)
            {
                return BadRequest(new { message = "The user doesn't exist" });
            }

            if (categoryId <= 0)
            {
                return BadRequest(new { message = "This category do not exist" });
            }

            var aiResponsees = await _ollamaService.GetAiResponsesAsync(userId, categoryId);

            if (aiResponsees  == null )
            {
                return NotFound();
            }

            return Ok(aiResponsees);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadCsv([FromForm] AiResponseDto aiResponseDto)
        {
            if (aiResponseDto.File == null || aiResponseDto.File.Length == 0)
            {
                return BadRequest(new { message = "Invalid file. Please upload a non-empty CSV file." });
            }

            if (aiResponseDto.UserId <= 0)
            {
                return BadRequest(new { message = "The user doesn't exist" });
            }

            if (aiResponseDto.CategoryId <= 0)
            {
                return BadRequest(new { message = "This category do not exist" });
            }

            using var stream = new MemoryStream();

            await aiResponseDto.File.CopyToAsync(stream);

            stream.Position = 0;

            // Parse the CSV file to JSON
            string jsonData = _csvParserService.ParseCsvToJson(stream);

            // Send the JSON data to the Ollama service for interpretation
            JsonDocument ollamaResponse = await _ollamaService.InterpretJson(jsonData, aiResponseDto.CategoryId);

            // Save the parsed data to the database
            //await _ollamaService.CreateAiResponse(ollamaResponse, aiResponse);

            return Ok(ollamaResponse);
        }
    }
}




