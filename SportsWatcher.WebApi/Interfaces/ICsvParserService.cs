namespace SportsWatcher.WebApi.Interfaces
{
    public interface ICsvParserService
    {
        string ParseCsvToJson(Stream csvStream);
    }
}
