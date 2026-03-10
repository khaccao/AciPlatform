using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("WareHouseShelvesWithFloors")]
public class WareHouseShelvesWithFloors
{
    [Key]
    public int Id { get; set; }
    public int WareHouseShelvesId { get; set; }
    public int WareHouseFloorId { get; set; }
}
