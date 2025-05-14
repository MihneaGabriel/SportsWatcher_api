namespace SportsWatcher.WebApi.DTOs
{
    public class AiResponseDto
    {
        public required IFormFile File {  get; set; }
        public required int UserId { get; set; }
        public required int CategoryId { get; set; }
    }
}
