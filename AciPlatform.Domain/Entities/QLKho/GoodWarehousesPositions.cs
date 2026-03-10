using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("GoodWarehousesPositions")]
public class GoodWarehousesPositions
{
    [Key]
    public int Id { get; set; }
    
    public int GoodWarehousesId { get; set; }
    
    [MaxLength(50)]
    public string? Warehouse { get; set; }
    
    public int WareHouseShelvesId { get; set; }
    public int WareHouseFloorId { get; set; }
    public int WareHousePositionId { get; set; }
    
    public double Quantity { get; set; }
}
