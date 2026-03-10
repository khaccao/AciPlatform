using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("GoodWarehouses")]
public class GoodWarehouses
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string? MenuType { get; set; }
    
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
    
    [MaxLength(50)]
    public string? GoodsType { get; set; }
    
    [MaxLength(255)]
    public string? Image1 { get; set; }
    
    public double Quantity { get; set; }
    public double QuantityInput { get; set; }
    
    public int Status { get; set; }
    
    [MaxLength(50)]
    public string? PriceList { get; set; }
    
    public int Order { get; set; }
    
    [MaxLength(50)]
    public string? OrginalVoucherNumber { get; set; }
    
    public int? LedgerId { get; set; }
    
    public string? Note { get; set; }
    
    public DateTime? DateExpiration { get; set; }
    public DateTime? DateManufacture { get; set; }
    
    public bool IsPrinted { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; } = false;
}
