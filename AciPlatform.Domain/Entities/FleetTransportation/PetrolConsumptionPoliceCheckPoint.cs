using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.FleetTransportation;

[Table("PetrolConsumptionPoliceCheckPoints")]
public class PetrolConsumptionPoliceCheckPoint
{
    [Key]
    public int Id { get; set; }

    public int PetrolConsumptionId { get; set; }

    [MaxLength(255)]
    public string? PoliceCheckPointName { get; set; }

    public double Amount { get; set; }

    public bool IsArise { get; set; }
}
