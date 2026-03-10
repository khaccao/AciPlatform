using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("Inventories")]
public class Inventory
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string? Account { get; set; }
    
    [MaxLength(255)]
    public string? AccountName { get; set; }
    
    [MaxLength(50)]
    public string? Warehouse { get; set; }
    
    [MaxLength(255)]
    public string? WarehouseName { get; set; }
    
    [MaxLength(50)]
    public string? Detail1 { get; set; }
    
    [MaxLength(255)]
    public string? DetailName1 { get; set; }
    
    [MaxLength(50)]
    public string? Detail2 { get; set; }
    
    [MaxLength(255)]
    public string? DetailName2 { get; set; }
    
    [MaxLength(255)]
    public string? Image1 { get; set; }
    
    public double InputQuantity { get; set; } = 0;
    public double OutputQuantity { get; set; } = 0;
    public double CloseQuantity { get; set; } = 0;
    public double CloseQuantityReal { get; set; } = 0;
    
    public DateTime? CreateAt { get; set; } = DateTime.Now;
    public string? Note { get; set; }
    public DateTime? DateExpiration { get; set; }
    public bool isCheck { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}
