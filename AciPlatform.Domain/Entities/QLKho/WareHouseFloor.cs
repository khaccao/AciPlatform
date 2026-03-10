using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.QLKho;

[Table("WareHouseFloors")]
public class WareHouseFloor
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string? Code { get; set; }
    
    [MaxLength(255)]
    public string? Name { get; set; }
    
    [MaxLength(250)]
    public string? Note { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public int? UserCreated { get; set; }
    public int? UserUpdated { get; set; }
    public bool IsDeleted { get; set; } = false;
}
