using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("CarLocations")]
public class CarLocation
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    [MaxLength(100)]
    public string? ProcedureNumber { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
