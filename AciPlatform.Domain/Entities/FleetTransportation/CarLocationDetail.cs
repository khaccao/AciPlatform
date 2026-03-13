using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("CarLocationDetails")]
public class CarLocationDetail
{
    [Key]
    public int Id { get; set; }

    public int CarLocationId { get; set; }

    [MaxLength(50)]
    public string? LicensePlates { get; set; }

    [MaxLength(100)]
    public string? Type { get; set; }

    [MaxLength(100)]
    public string? Payload { get; set; }

    [MaxLength(255)]
    public string? DriverName { get; set; }

    [MaxLength(500)]
    public string? Location { get; set; }

    [MaxLength(500)]
    public string? PlanInprogress { get; set; }

    [MaxLength(500)]
    public string? PlanExpected { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public string? FileStr { get; set; }

    public bool IsDeleted { get; set; } = false;
}
