using System.ComponentModel.DataAnnotations.Schema;

namespace SportsWatcher.WebApi.Entities.Nomenclature
{
    [Table("Categories", Schema = "nom")]
    public class Category : BaseNomenclatureEntity
    {
    }
}
