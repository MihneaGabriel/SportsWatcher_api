using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using SportsWatcher.WebApi.Interfaces;
using System.Globalization;

namespace SportsWatcher.WebApi.Services
{
    public class CsvParserService : ICsvParserService
    {
        public string ParseCsvToJson(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<dynamic>().ToList();
            return JsonConvert.SerializeObject(records, Formatting.Indented);
        }
    }
}
