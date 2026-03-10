using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("GoodWarehouseExports")]
public class GoodWarehouseExport
{
    [Key]
    public int Id { get; set; }
    
    public int GoodWarehouseId { get; set; }
    
    public int BillId { get; set; }
    
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    
    public bool IsDeleted { get; set; } = false;
}
