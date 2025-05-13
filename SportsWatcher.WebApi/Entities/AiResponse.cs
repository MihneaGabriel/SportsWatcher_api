using System.Text.Json;

namespace SportsWatcher.WebApi.Entities
{
    public class AiResponse : BaseEntity
    {
        public string JsonResponse { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int CategoryId { get; set; }
    }
}
