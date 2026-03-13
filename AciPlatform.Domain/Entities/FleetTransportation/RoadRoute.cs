using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("RoadRoutes")]
public class RoadRoute
{
    [Key]
    public int Id { get; set; }

    [MaxLength(255)]
    public string? Code { get; set; }

    [MaxLength(255)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? RoadRouteDetail { get; set; }

    public string? PoliceCheckPointIdStr { get; set; }

    public double NumberOfTrips { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
