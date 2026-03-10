using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("WareHouseFloorWithPositions")]
public class WareHouseFloorWithPosition
{
    [Key]
    public int Id { get; set; }
    public int WareHouseFloorId { get; set; }
    public int WareHousePositionId { get; set; }
}
