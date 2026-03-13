using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("PetrolConsumptions")]
public class PetrolConsumption
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public int CarId { get; set; }

    public double PetroPrice { get; set; }

    public double KmFrom { get; set; }

    public double KmTo { get; set; }

    [MaxLength(500)]
    public string? LocationFrom { get; set; }

    [MaxLength(500)]
    public string? LocationTo { get; set; }

    public double AdvanceAmount { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public int? RoadRouteId { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
