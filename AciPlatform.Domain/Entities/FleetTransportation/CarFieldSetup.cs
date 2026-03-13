using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("CarFieldSetups")]
public class CarFieldSetup
{
    [Key]
    public int Id { get; set; }

    public int CarId { get; set; }

    public int CarFieldId { get; set; }

    public double? ValueNumber { get; set; }

    public DateTime? FromAt { get; set; }

    public DateTime? ToAt { get; set; }

    public DateTime? WarningAt { get; set; }

    [MaxLength(500)]
    public string? UserIdString { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    [MaxLength(1000)]
    public string? FileLink { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
