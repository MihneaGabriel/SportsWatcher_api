namespace SportsWatcher.WebApi.Entities.Nomenclature
{
    public class BaseNomenclatureEntity : BaseEntity
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
        public required bool isActive { get; set; }
    }
}
