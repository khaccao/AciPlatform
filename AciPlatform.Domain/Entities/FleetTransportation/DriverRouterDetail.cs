using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("DriverRouterDetails")]
public class DriverRouterDetail
{
    [Key]
    public int Id { get; set; }

    public int DriverRouterId { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public DateTime Date { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    [MaxLength(500)]
    public string? Location { get; set; }

    public decimal Amount { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public string? FileStr { get; set; }

    public int PoliceCheckPointId { get; set; }

    public bool IsDeleted { get; set; } = false;
}
