namespace SportsWatcher.WebApi.Entities
{
    public class AiResponse : BaseEntity
    {
        public string JsonResponse { get; set; }

        public int UserId { get; set; }
    }
}
