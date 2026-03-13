using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("CarFields")]
public class CarField
{
    [Key]
    public int Id { get; set; }

    public int CarId { get; set; }

    public int Order { get; set; }

    [MaxLength(255)]
    public string? Name { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
