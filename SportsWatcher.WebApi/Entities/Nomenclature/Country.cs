using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWatcher.WebApi.Entities.Nomenclature
{
    [Table("Countries", Schema = "nom")]
    public class Country : BaseNomenclatureEntity
    {
    }
}
