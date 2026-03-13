using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("Cars")]
public class Car
{
    [Key]
    public int Id { get; set; }

    [MaxLength(36)]
    public string? LicensePlates { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    [MaxLength(1000)]
    public string? FileLink { get; set; }

    public string? Content { get; set; }

    public double MileageAllowance { get; set; }

    public double FuelAmount { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
