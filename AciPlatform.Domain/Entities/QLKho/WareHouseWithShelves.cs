using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("WareHouseWithShelves")]
public class WareHouseWithShelves
{
    [Key]
    public int Id { get; set; }
    public int WareHouseId { get; set; }
    public int WareHouseShelveId { get; set; }
}
